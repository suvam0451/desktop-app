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
//using namespace System::Windows::Markup;
using namespace System::Collections::Generic;
using namespace System::Text::RegularExpressions;
//using namespace System::Windows::Xaml;
//using namespace System::Windows::Input;
//using namespace System::Windows::Presentation;
//using namespace System::Windows::
//using namespace msclr
//using namespace PresentationCore;
//using namespace System::Windows::Controls;

// Threading operations...
using namespace System::Threading;
using namespace System::Windows::Threading;

#include <string>
#include <vector>
#include <opencv2/opencv.hpp>
#include "HelperLibrary.h"
//#include <boost/python/numpy.hpp>

// void __stdcall MyCallback(int percent);

// Callback functions(pure C++)...
// typedef void (__stdcall *CallbackFunc)(int);
// CallbackFunctions in C#
//namespace p = boost::python;
//namespace np = boost::python;

// call this directly from c# with method adequate to the action
// void TestCallBack(Action<int>^);
// call this directly from C# with method adequate to the handler
// void TestCallBack2(ManagedCallbackHandler^);		
public delegate void ManagedCallbackHandler(int _In);
public delegate void ImageDelegate(ImageSource^ in);


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
			//return 10;

			return gcnew System::String("oniichan");
		}

		void PlayMovie() {
			
		}

		void HandleMediaDrop(array<String^>^ strarr) {
			cv::VideoCapture cap(HelperLibrary::StringManagedToSTL(strarr[0]));
			cap.set(cv::CAP_PROP_FPS, 60);
			
			if (cap.isOpened()) {
				// Handle not video file error
			}
			while (true) {
				cv::Mat frame;
				cap >> frame;
				if (frame.empty()) break;
				
				// Decorator info text
				cv::putText(frame, "Tortoise", cv::Point(10, 30), cv::FONT_HERSHEY_COMPLEX, 0.6, (0,255,0), 2);
				// Set image
				UpdateDisplay(HelperLibrary::GetImageSourceFromCV(frame));
				// set console
				cap.get(cv::CAP_PROP_FPS).ToString();

				UpdateConsole(cap.get(cv::CAP_PROP_FPS).ToString());
				// ESC to exit
				char c = (char)cv::waitKey(16.66);
				if (c == 27) break;
			}
			// Release memory
			cap.release();
			cv::destroyAllWindows();
			//MessageBox::Show(strarr[0]);
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


			//cv::Mat example = tmp1.clone();
			//tmp1[0];
			cv::hconcat(albedo, normal, tmp1);
			cv::hconcat(combined_01, combined_02, tmp2);

			// Vertical concatenation to form 4 quadrants
			cv::vconcat(tmp1, tmp2, out);

			// Update image on UI thread

			yamete = HelperLibrary::GetImageSourceFromCV(out);
			//UpdateDisplay(yamete);
			imagedel(yamete);

			//delete(retval);

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

		// Build sthe image to be displayed...
		void BuildDisplayImage() {
		}

		//void GetImageSourceFromCV(std::string name) {
		//
		//	String^ usethis;
		//
		//	FileData->TryGetValue("albedo", usethis);
		//	std::string str = HelperLibrary::StringManagedToSTL(usethis);
		//	cv::Mat In = cv::imread(str);
		//	// Convern OpenCV image to Bitmap...
		//	System::IntPtr ptr(In.ptr());
		//	Bitmap^ bitmap = gcnew Bitmap(In.cols, In.rows, In.step, System::Drawing::Imaging::PixelFormat::Format24bppRgb, ptr);
		//
		//	// Stream image data to stream...
		//	MemoryStream^ ms = gcnew MemoryStream();
		//	bitmap->Save(ms, System::Drawing::Imaging::ImageFormat::Bmp);
		//
		//	// Declare a bitmap image and read the stream...
		//	BitmapImage^ image = gcnew BitmapImage();
		//	image->BeginInit();
		//	ms->Seek(0, SeekOrigin::Begin);
		//	image->StreamSource = ms;
		//	image->EndInit();
		//
		//	ImageSource^ retval = image;
		//	UpdateDisplay(image);
		//	//_dispatcherref->BeginInvoke(gcnew UpdateImage(this, &TextureCombine_Type1_Backend::UpdateDisplay), image);
		//	//_dispatcherref->BeginInvoke(gcnew ConsoleUpdateDel(this, &TextureCombine_Type1_Backend::UpdateConsole), "chotto matte");
		//
		//	//current->Sleep(5);
		//	//SetImage(image);
		//
		//	//_dispatcherref->BeginInvoke(_displayref->Source = in);
		//	//_dispatcherref->BeginInvoke(gcnew ImageUpdate(this->SetImage), retval);
		//	//this->Invoke(gcnew ImageUpdate(this, &TextureCombine_Type1_Backend::SetImage), retval);
		//	//_displayref->Source = image;
		//
		//	// Thread to set image asset...
		//	//ThreadStart^ del = gcnew ThreadStart(this, &TextureCombine_Type1_Backend::GetImageSourceFromCV);	
		//	//Thread^ thread = gcnew Thread(del);
		//	//thread->Start();
		//	//GetImageSourceFromCV("albedo");
		//	//SetImageSourceFromCVDel^ del = gcnew SetImageSourceFromCVDel(this, &TextureCombine_Type1_Backend::GetImageSourceFromCV);
		//
		//	//Thread^ thread = gcnew Thread(GetImageSourceFromCV);
		//	//gcnew ThreadStart(this, &GetImageSourceFromCV(img))
		//	//_displayref->Source = HelperLibrary::GetImageSourceFromCV(img);
		//	
		//	// gc
		//
		//	// OpenCV
		//	In.deallocate();
		//	In.release();
		//}

	private:
		void UpdateConsole(String^ in) {
			_consoleref->Text = in;
		}
		void UpdateDisplay(ImageSource^ in) {
			delete(_displayref->Source);
			_displayref->Source = in;
		}

	protected:
		!TextureCombine_Type1_Backend() {}
	};
};