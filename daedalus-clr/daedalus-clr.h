#pragma once

using namespace System::Runtime::InteropServices;
using namespace System::Windows;
using namespace System::IO;
using namespace System::Drawing;
using namespace System::Windows::Media;
using namespace System::Windows::Media::Imaging;
using namespace System::Windows::Controls;
using namespace System::Windows::Interop;
using namespace System::Windows::Media;
using namespace System::Collections::Generic;
using namespace System::Text::RegularExpressions;
using namespace System::Threading;
using namespace System::Windows::Threading;

#include <string>
#include <vector>
#include <opencv2/opencv.hpp>
#include "HelperLibrary.h"
#include <OpenXLSX/OpenXLSX.h>

#define CVColors const cv::Scalar SCALAR_BLACK = cv::Scalar(0.0, 0.0, 0.0); \
		const cv::Scalar SCALAR_WHITE = cv::Scalar(255.0, 255.0, 255.0); \
		const cv::Scalar SCALAR_BLUE = cv::Scalar(255.0, 0.0, 0.0); \
		const cv::Scalar SCALAR_GREEN = cv::Scalar(0.0, 200.0, 0.0); \
		const cv::Scalar SCALAR_RED = cv::Scalar(0.0, 0.0, 255.0);

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

		// Constructor/Destructor...
		TextureCombine_Type1_Backend() {
			FileData = gcnew Dictionary<String^, String^>();
		}
		~TextureCombine_Type1_Backend();



		/* Image Processinng function called from the wpf module. */
		void HandleMediaDrop(String^ strarr);

		/*  Image combiner multiple image files drop. */
		void HandleFileDrop(ImageDelegate^ imagedel, array<String^>^ strarr);

		// OpenCV helper functions
		void matchCurrentFrameBlobsToExistingBlobs(std::vector<Blob>& existingBlobs, std::vector<Blob>& currentFrameBlobs);
		void addBlobToExistingBlobs(Blob& currentFrameBlob, std::vector<Blob>& existingBlobs, int& intIndex);
		void addNewBlob(Blob& currentFrameBlob, std::vector<Blob>& existingBlobs);
		double distanceBetweenPoints(cv::Point point1, cv::Point point2);
		void drawAndShowContours(cv::Size imageSize, std::vector<std::vector<cv::Point>> contours, std::string strImageName);
		void drawAndShowContours(cv::Size imageSize, std::vector<Blob> blobs, std::string strImageName);
		bool checkIfBlobsCrossedTheLine(std::vector<Blob>& blobs, int& intHorizontalLinePosition, int& carCount);
		void drawBlobInfoOnImage(std::vector<Blob>& blobs, cv::Mat& imgFrame2Copy);
		void drawCarCountOnImage(int& carCount, cv::Mat& imgFrame2Copy);


		// void UpdateConsole(String^ in) { _consoleref->Text = in; }

	protected:
		!TextureCombine_Type1_Backend() {}
	};
};