#pragma once

// Used for inter-operatibility
using namespace System::Runtime::InteropServices;
// Used for messagebox
using namespace System::Windows;

// Media handling...
using namespace System::IO;
using namespace System::Drawing;
using namespace System::Windows::Media;
using namespace System::Windows::Media::Imaging;

using namespace System::Windows::Controls;
using namespace System::Windows::Interop;
using namespace System::Windows::Media;
using namespace System::Collections::Generic;
using namespace System::Text::RegularExpressions;

// Threading operations...
using namespace System::Threading;
using namespace System::Windows::Threading;

#include <string>
#include <vector>
#include <opencv2/opencv.hpp>
#include "HelperLibrary.h"
#include <lua.hpp>

public delegate void ManagedCallbackHandler(int _In);
public delegate void ImageDelegate(ImageSource^ In);

namespace daedalus_clr {
	public ref class TextureCombine_Type1_Backend
	{
	private:
		Dictionary<String^, String^>^ FileData;

		delegate void ConsoleUpdateDel(String^ in);
		delegate void ImageUpdateDel(ImageSource^ in);
		delegate void SetImageSourceFromCVDel(String^ in);
		delegate void UpdateImage(ImageSource^ in);

	public:
		int _size = 1024;

		TextBlock^ _consoleref;
		ImageSource^ imgsrc;
		ConsoleUpdateDel^ _consoleDel;

	public:
#pragma region Constructor/ Destructor

		// Constructor/Destructor...
		TextureCombine_Type1_Backend() {
			FileData = gcnew Dictionary<String^, String^>();
		}
		~TextureCombine_Type1_Backend() {
			// Delete heap allocated resources...
			FileData->Clear();
			delete(_consoleref);
			delete(imgsrc);
			// Call post-delete...
			this->!TextureCombine_Type1_Backend();
		}

#pragma endregion
		void GetDataSize() {
			_consoleref->Text = FileData->Count.ToString();
		}

		static String^ GetCodes(System::Object^ sender, System::ComponentModel::PropertyChangedEventArgs^ e) {
			return gcnew System::String("oniichan");
		}

		void PlayMovie() {
			
		}

		/*
			Image Processinng function called from the wpf module.
		*/
		void HandleMediaDrop(String^ strarr)
		{


			/*
			*	OpenCV Initialization
			*/
			std::string videoPath = HelperLibrary::StringManagedToSTL(strarr);
			cv::VideoCapture cap;
			cv::Mat imgFrame1;
			cv::Mat imgFrame2;
			std::vector<Blob> blobs;
			char chCheckForEscKey = 0;

			cap.open(videoPath);

			/*
			*	State initialization using lua config file
			*/
			lua_State* L = luaL_newstate();
			luaL_openlibs(L);
			luaL_dofile(L, "lewdheh.lua");
			lua_getglobal(L, "max_frames");
			cap.set(cv::CAP_PROP_FPS, lua_tonumber(L, -1));
			lua_close(L);

			
			/*
			*	OpenCV global variables
			*/
			const cv::Scalar SCALAR_BLACK = cv::Scalar(0.0, 0.0, 0.0);
			const cv::Scalar SCALAR_WHITE = cv::Scalar(255.0, 255.0, 255.0);
			const cv::Scalar SCALAR_BLUE = cv::Scalar(255.0, 0.0, 0.0);
			const cv::Scalar SCALAR_GREEN = cv::Scalar(0.0, 200.0, 0.0);
			const cv::Scalar SCALAR_RED = cv::Scalar(0.0, 0.0, 255.0);

			/*
			*	OpenCV Error handling
			*/
			if (!cap.isOpened()) {
				std::cout << "\nerror reading video file" << std::endl << std::endl;      // show error message
				return;
			}

			if (cap.get(cv::CAP_PROP_FRAME_COUNT) < 2) {
				std::cout << "\nerror: video file must have at least two frames";
				return;
			}

			// cap.read(imgFrame1);
			// cap.read(imgFrame2);


			/* 
			*	OPENCV video loop 
			*/
			while (cap.isOpened() && chCheckForEscKey != 27)
			{
				cap >> imgFrame1; // Alternatively cap.read()
				cap >> imgFrame2;
				// set console
				cap.get(cv::CAP_PROP_FPS).ToString();

				cv::Mat imgFrame1Copy = imgFrame1.clone();
				cv::Mat imgFrame2Copy = imgFrame2.clone();

				cv::Mat imgDifference;
				cv::Mat imgThresh;

				cv::cvtColor(imgFrame1Copy, imgFrame1Copy, cv::COLOR_BGR2GRAY);
				cv::cvtColor(imgFrame2Copy, imgFrame2Copy, cv::COLOR_BGR2GRAY);

				cv::GaussianBlur(imgFrame1Copy, imgFrame1Copy, cv::Size(5, 5), 0);
				cv::GaussianBlur(imgFrame2Copy, imgFrame2Copy, cv::Size(5, 5), 0);
				
				cv::absdiff(imgFrame1Copy, imgFrame2Copy, imgDifference);
				
				cv::threshold(imgDifference, imgThresh, 30, 255, cv::THRESH_BINARY);

				cv::imshow("imgThresh", imgThresh);

				cv::Mat structuringElement3x3 = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(3, 3));
				cv::Mat structuringElement5x5 = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(5, 5));
				cv::Mat structuringElement7x7 = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(7, 7));
				cv::Mat structuringElement9x9 = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(9, 9));

				cv::dilate(imgThresh, imgThresh, structuringElement5x5);
				cv::dilate(imgThresh, imgThresh, structuringElement5x5);
				cv::erode(imgThresh, imgThresh, structuringElement5x5);

				cv::Mat imgThreshCopy = imgThresh.clone();

				std::vector<std::vector<cv::Point>> contours;

				cv::findContours(imgThreshCopy, contours, cv::RETR_EXTERNAL, cv::CHAIN_APPROX_SIMPLE);
				
				cv::Mat imgContours(imgThresh.size(), CV_8UC3, SCALAR_BLACK);

				cv::drawContours(imgThresh, contours, -1, SCALAR_WHITE, -1);

				cv::imshow("imgContours", imgContours);

				std::vector<std::vector<cv::Point>> convexHulls(contours.size());

				for (unsigned int i = 0; i < contours.size(); i++) {
					cv::convexHull(contours[i], convexHulls[i]);
				}

				for (auto& convexHull : convexHulls) {
					Blob possibleBlob(convexHull);

					if (possibleBlob.boundingRect.area() > 100 &&
						possibleBlob.dblAspectRatio >= 0.2 &&
						possibleBlob.dblAspectRatio <= 1.2 &&
						possibleBlob.boundingRect.width > 15 &&
						possibleBlob.boundingRect.height > 20 &&
						possibleBlob.dblDiagonalSize > 30.0)
					{
						blobs.push_back(possibleBlob);
					}
				}

				cv::Mat imgConvexHulls(imgThresh.size(), CV_8UC3, SCALAR_BLACK);

				convexHulls.clear();

				for (auto& blob : blobs) {
					convexHulls.push_back(blob.contour);
				}

				cv::drawContours(imgConvexHulls, convexHulls, -1, SCALAR_WHITE, -1);

				cv::imshow("imgConvexHulls", imgConvexHulls);
					
				// get another copy of frame 2 since we changed the previous frame 2 copy in the processing above
				imgFrame2Copy = imgFrame2.clone();

				// Draws box around blobs, circle at center
				for (auto& blob : blobs) {
					cv::rectangle(imgFrame2Copy, blob.boundingRect, SCALAR_RED, 2);
					cv::circle(imgFrame2Copy, blob.centerPosition, 3, SCALAR_GREEN, -1);		
				}

				cv::imshow("imgFrame2Copy", imgFrame2Copy);

				imgFrame1 = imgFrame2.clone(); // Move frame 1 upto frame 2

				if ((cap.get(cv::CAP_PROP_POS_FRAMES) + 1)
					< cap.get(cv::CAP_PROP_FRAME_COUNT)) {
					cap.read(imgFrame2);
				}
				else {
					break;
				}

				chCheckForEscKey = cv::waitKey(1);
			}
				// char c = (char)cv::waitKey(1);
				// if (c == 27) 
				//	break;
			// Release memory
			cap.release();
			cv::destroyAllWindows();
		}

		// Handles event when items are dropped...
		void HandleFileDrop(ImageDelegate^ imagedel, array<String^>^ strarr)
		{
			FileData->Clear();

			for each (String^ it in strarr)
			{
				Regex^ rx;
				MatchCollection^ matches;

				rx = gcnew Regex("(albedo|color)+(.?)+(.png|.jpg)", RegexOptions::Compiled | RegexOptions::IgnoreCase);
				matches = rx->Matches(it);
				if (matches->Count > 0) { FileData->Add("albedo", it); continue; }

				rx = gcnew Regex("(normal)+(.?)+(.png|.jpg)", RegexOptions::IgnoreCase | RegexOptions::Compiled);
				matches = rx->Matches(it);
				if (matches->Count > 0) { FileData->Add("normal", it); continue; }

				rx = gcnew Regex("(height|displacement|bump)+(.?)+(.png|.jpg)", RegexOptions::IgnoreCase | RegexOptions::Compiled);
				matches = rx->Matches(it);
				if (matches->Count > 0) { FileData->Add("height", it); continue; }

				rx = gcnew Regex("(ao|ambient|occlusion)+(.?)+(.png|.jpg)", RegexOptions::IgnoreCase | RegexOptions::Compiled);
				matches = rx->Matches(it);
				if (matches->Count > 0) { FileData->Add("ao", it); continue; }

				rx = gcnew Regex("(rough)+(.?)+(.png|.jpg)", RegexOptions::IgnoreCase | RegexOptions::Compiled);
				matches = rx->Matches(it);
				if (matches->Count > 0) { FileData->Add("roughness", it); continue; }

				rx = gcnew Regex("(metal)+(.?)+(.png|.jpg)", RegexOptions::IgnoreCase | RegexOptions::Compiled);
				matches = rx->Matches(it);
				if (matches->Count > 0) { FileData->Add("metallic", it); continue; }
			}

			String^ res;
			// Allocate matrices. Fallback to defaulf if not caught...
			cv::Mat albedo = (FileData->TryGetValue("albedo", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
			cv::Mat normal = (FileData->TryGetValue("normal", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
			cv::Mat height = (FileData->TryGetValue("height", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
			cv::Mat rough = (FileData->TryGetValue("rough", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
			cv::Mat metal = (FileData->TryGetValue("metal", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
			cv::Mat ao = (FileData->TryGetValue("ao", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));

			cv::Mat tmp1, tmp2, out, combined_01, combined_02;
			cv::Mat* tmparr = new cv::Mat[3];
			cv::Mat* writearr = new cv::Mat[3];

			// Start combining first combined array...
			cv::split(height, tmparr);
			writearr[0] = tmparr[0];
			writearr[1] = tmparr[0];
			writearr[2] = tmparr[0];
			cv::merge(writearr, 3, combined_01);


			// Start combining second combined array...
			cv::split(ao, tmparr);
			writearr[0] = tmparr[0];
			cv::split(rough, tmparr);
			writearr[1] = tmparr[1];
			cv::split(metal, tmparr);
			writearr[2] = tmparr[2];
			cv::merge(writearr, 3, combined_02);

			cv::hconcat(albedo, normal, tmp1);
			cv::hconcat(combined_01, combined_02, tmp2);

			// Vertical concatenation to form 4 quadrants
			cv::vconcat(tmp1, tmp2, out);

			imgsrc = HelperLibrary::GetImageSourceFromCV(out);

			MessageBox::Show("Used to meet in the eastside");
			imagedel(imgsrc);
			// consoleDel(gcnew String("Completed Task"));

			albedo.release();
			normal.release();
			height.release();
			rough.release();
			metal.release();
			ao.release();
			// ---
			tmp1.release();
			tmp2.release();
			out.release();
			combined_01.release();
			combined_02.release();

			// --- (Not pointing to null causes crash...)
			tmparr = nullptr;
			delete(tmparr);
			writearr = nullptr;
			delete(writearr);
			//delete(tmparr);
			//delete(writearr);
		}

	private:
		void UpdateConsole(String^ in)
		{
			_consoleref->Text = in;
		}

	protected:
		!TextureCombine_Type1_Backend() {}
	};
};