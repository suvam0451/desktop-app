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
		// This array holds the paths for valid files in dictionary form.
		Dictionary<String^, String^>^ FileData;

		//HelperLibrary inst = new HelperLibrary;
		// Function delegates...
		delegate void ConsoleUpdateDel(String^ in);
		delegate void ImageUpdateDel(ImageSource^ in);
		delegate void SetImageSourceFromCVDel(String^ in);
		delegate void UpdateImage(ImageSource^ in);



		// declare unmanaged fucntion type
		typedef int(__stdcall* ANSWERCB)(int, int);
		static ANSWERCB cb;
	public:
		// This is the console that should be automatically updated..
		int _size = 1024;

		TextBlock^ _consoleref;
		// This is the display board
		Windows::Controls::Image^ _displayref;

		// State to be referenced...
		String^ ConsoleStatus;
		ImageSource^ imgsrc;
		BitmapImage^ yamete;
		Dispatcher^ _dispatcherref = nullptr;
		//ImageSource^ retval;

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
			delete(_displayref);
			delete(FileData);
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

		void HandleMediaDrop(String^ strarr)
		{
			lua_State* L = luaL_newstate();
			luaL_openlibs(L);
			luaL_dofile(L, "lewdheh.lua");

			cv::VideoCapture cap(HelperLibrary::StringManagedToSTL(strarr));

			lua_getglobal(L, "max_frames");
			// MessageBox::Show(lua_tonumber(L, -1).ToString());
			cap.set(cv::CAP_PROP_FPS, lua_tonumber(L, -1));	

			lua_close(L);

			int mincost = INT32_MAX;
			int** connectivity;
			int minindex = 10;
			// Vic and jesco, what do i doooo :(
			(connectivity[0][0] == 1) ? (minindex = 10) : (minindex = 0);
			
			
			// for (int i = 0; i < Vertices; i++) {

				// TERNARY ASSIGNMENTS : works
				// This works. good.
				// mincost = (connectivity[a, i] == 1) ? Math.Min(cost[a, i], mincost) : mincost;

				// This works. I can live with that.
				// _ = (connectivity[a, i] == 1) ? Math.Min(cost[a, i], mincost) : mincost;

				// TERNARY EXPRESSIONS : do not work
				// This does not work. Damn it C#
				// (connectivity[a, i] == 1) ? Math.Min(cost[a, i], mincost) : true;

				// This also does not work. This would have worked in C++
				

				// Vic and jesco, what do i doooo :(
			//}



			if (cap.isOpened()) {
				// Handle not video file error
			}
			while (true) {
				cv::Mat frame;
				cap >> frame;
				if (frame.empty())
					break;
				
				cv::imshow("RGB", frame);

				// Decorator info text
				cv::putText(frame, "Tortoise", cv::Point(10, 30), cv::FONT_HERSHEY_COMPLEX, 0.6, (0,255,0), 2);

				// set console
				cap.get(cv::CAP_PROP_FPS).ToString();

				// Denoise
				// cv::Mat output;
				// cv::GaussianBlur(frame, output, cv::Size(3, 3), 0, 0);

				frame = LaneDetector::deNoise(frame);
				// frame = LaneDetector::edgeDetector(frame);

				// cv::Mat output;
				// cv::Mat kernel;
				// cv::Point anchor;
				// Convert image from RGB to gray
				
				// Binarize gray image
				// cv::threshold(output, output, 140, 255, cv::THRESH_BINARY);

				// cv::Mat newout = LaneDetector::edgeDetector(frame);

				cv::Mat vicky;

				// cv::cvtColor(frame, vicky, cv::COLOR_RGB2GRAY);
				// Create the kernel [-1 0 1]
				// This kernel is based on the one found in the
				// Lane Departure Warning System by Mathworks
				// anchor = cv::Point(-1, -1);
				// kernel = cv::Mat(1, 3, CV_32F);
				// kernel.at<float>(0, 0) = -1;
				// kernel.at<float>(0, 1) = 0;
				// kernel.at<float>(0, 2) = 1;
				// Filter the binary image to obtain the edges
				// cv::filter2D(output, output, -1, kernel, anchor, 0, cv::BORDER_DEFAULT);
				

				// UpdateConsole(cap.get(cv::CAP_PROP_FPS).ToString());
				UpdateDisplay(HelperLibrary::GetImageSourceFromCV(frame));

				char c = (char)cv::waitKey(1);
				if (c == 27) 
					break;
			}
			// Release memory
			cap.release();
			cv::destroyAllWindows();
		}

		// Handles event when items are dropped...
		void HandleFileDrop(ImageDelegate^ imagedel, array<String^>^ strarr) {
			
			FileData->Clear();

			// Check for maps using pattern matching...
			for each (String^ it in strarr)
			{
				Regex^ rx;
				MatchCollection^ matches;

				// Check for maps...
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

				// local gc
				delete(rx);
				delete(matches);
			}

			_dispatcherref->BeginInvoke(gcnew ConsoleUpdateDel(this, &TextureCombine_Type1_Backend::UpdateConsole), "Completed");

			String^ res;
			// Allocate matrices. Fallback to defaulf if not caught...
			cv::Mat albedo = (FileData->TryGetValue("albedo", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
			cv::Mat normal = (FileData->TryGetValue("normal", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
			cv::Mat height = (FileData->TryGetValue("height", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
			cv::Mat rough = (FileData->TryGetValue("rough", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
			cv::Mat metal = (FileData->TryGetValue("metal", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
			cv::Mat ao = (FileData->TryGetValue("ao", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));

			delete(res);

			// No need to allocate...
			cv::Mat tmp1, tmp2, out, combined_01, combined_02;

			// Temporary array to split channels...
			cv::Mat* tmparr = new cv::Mat[3];
			// Temporary array to hold channels to be merged...
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
			UpdateDisplay(imgsrc);

			// gc 
			// Using deallocate first WILL NOT not release memory...
			// Advised to use release alone...
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

		void UpdateDisplay(ImageSource^ in)
		{
			_displayref->Source = in;
		}

	protected:
		!TextureCombine_Type1_Backend() {}
	};
};