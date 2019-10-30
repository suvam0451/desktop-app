#include "pch.h"
#include "HelperLibrary.h"
using namespace daedalus_clr;

cv::Mat LaneDetector::deNoise(cv::Mat inputImage) {
	cv::Mat output;

	cv::GaussianBlur(inputImage, output, cv::Size(3, 3), 0, 0);
	return output;
}

cv::Mat LaneDetector::edgeDetector(cv::Mat img_noise) {
	cv::Mat output;
	cv::Mat kernel;
	cv::Point anchor;

	// convert to rgb array
	cv::cvtColor(img_noise, output, cv::COLOR_RGB2GRAY);
	// Binarize gray image
	cv::threshold(output, output, 140, 255, cv::THRESH_BINARY);

	// Create the kernel [-1 0 1]
	// This kernel is based on the one found in the
	// Lane Departure Warning System by Mathworks
	anchor = cv::Point(-1, -1);
	kernel = cv::Mat(1, 3, CV_32F);
	kernel.at<float>(0, 0) = -1;
	kernel.at<float>(0, 1) = 0;
	kernel.at<float>(0, 2) = 1;

	// Filter binary image to obtain edges
	cv::filter2D(output, output, -1, kernel, anchor, 0, cv::BORDER_DEFAULT);

	return output;
}

#pragma region Blob

Blob::Blob(std::vector<cv::Point> _contour)
{
	currentContour = _contour;

	currentBoundingRect = cv::boundingRect(currentContour);

	cv::Point currentCenter;

	currentCenter.x = (currentBoundingRect.x + currentBoundingRect.x + currentBoundingRect.width) / 2;
	currentCenter.y = (currentBoundingRect.y + currentBoundingRect.y + currentBoundingRect.height) / 2;

	centerPositions.push_back(currentCenter);

	dblCurrentDiagonalSize = sqrt(pow(currentBoundingRect.width, 2) + pow(currentBoundingRect.height, 2));

	dblCurrentAspectRatio = (float)currentBoundingRect.width / (float)currentBoundingRect.height;

	blnStillBeingTracked = true;
	blnCurrentMatchFoundOrNewBlob = true;

	intNumOfConsecutiveFramesWithoutAMatch = 0;
}

///////////////////////////////////////////////////////////////////////////////////////////////////
void Blob::predictNextPosition(void) {

	int numPositions = (int)centerPositions.size();

	if (numPositions == 1) {

		predictedNextPosition.x = centerPositions.back().x;
		predictedNextPosition.y = centerPositions.back().y;

	}
	else if (numPositions == 2) {

		int deltaX = centerPositions[1].x - centerPositions[0].x;
		int deltaY = centerPositions[1].y - centerPositions[0].y;

		predictedNextPosition.x = centerPositions.back().x + deltaX;
		predictedNextPosition.y = centerPositions.back().y + deltaY;

	}
	else if (numPositions == 3) {

		int sumOfXChanges = ((centerPositions[2].x - centerPositions[1].x) * 2) +
			((centerPositions[1].x - centerPositions[0].x) * 1);

		int deltaX = (int)std::round((float)sumOfXChanges / 3.0);

		int sumOfYChanges = ((centerPositions[2].y - centerPositions[1].y) * 2) +
			((centerPositions[1].y - centerPositions[0].y) * 1);

		int deltaY = (int)std::round((float)sumOfYChanges / 3.0);

		predictedNextPosition.x = centerPositions.back().x + deltaX;
		predictedNextPosition.y = centerPositions.back().y + deltaY;

	}
	else if (numPositions == 4) {

		int sumOfXChanges = ((centerPositions[3].x - centerPositions[2].x) * 3) +
			((centerPositions[2].x - centerPositions[1].x) * 2) +
			((centerPositions[1].x - centerPositions[0].x) * 1);

		int deltaX = (int)std::round((float)sumOfXChanges / 6.0);

		int sumOfYChanges = ((centerPositions[3].y - centerPositions[2].y) * 3) +
			((centerPositions[2].y - centerPositions[1].y) * 2) +
			((centerPositions[1].y - centerPositions[0].y) * 1);

		int deltaY = (int)std::round((float)sumOfYChanges / 6.0);

		predictedNextPosition.x = centerPositions.back().x + deltaX;
		predictedNextPosition.y = centerPositions.back().y + deltaY;

	}
	else if (numPositions >= 5) {

		int sumOfXChanges = ((centerPositions[numPositions - 1].x - centerPositions[numPositions - 2].x) * 4) +
			((centerPositions[numPositions - 2].x - centerPositions[numPositions - 3].x) * 3) +
			((centerPositions[numPositions - 3].x - centerPositions[numPositions - 4].x) * 2) +
			((centerPositions[numPositions - 4].x - centerPositions[numPositions - 5].x) * 1);

		int deltaX = (int)std::round((float)sumOfXChanges / 10.0);

		int sumOfYChanges = ((centerPositions[numPositions - 1].y - centerPositions[numPositions - 2].y) * 4) +
			((centerPositions[numPositions - 2].y - centerPositions[numPositions - 3].y) * 3) +
			((centerPositions[numPositions - 3].y - centerPositions[numPositions - 4].y) * 2) +
			((centerPositions[numPositions - 4].y - centerPositions[numPositions - 5].y) * 1);

		int deltaY = (int)std::round((float)sumOfYChanges / 10.0);

		predictedNextPosition.x = centerPositions.back().x + deltaX;
		predictedNextPosition.y = centerPositions.back().y + deltaY;

	}
	else {
		// should never get here
	}

}

#pragma endregion

std::string HelperLibrary::StringManagedToSTL(String^ In)
{
	const char* mine = (const char*)Marshal::StringToHGlobalAnsi(In).ToPointer();
	std::string retval = mine;
	Marshal::FreeHGlobal(IntPtr((void*)mine));
	//delete(mine);
	return retval;
}

void HelperLibrary::DummyFunction(String^ _In)
{
	ExcelHelper::UniformFactorMethod(_In, false);
}

BitmapImage^ HelperLibrary::GetImageSourceFromCV(cv::Mat& In) {
	// Convern OpenCV image to Bitmap...
	System::IntPtr ptr(In.ptr());
	Bitmap^ bitmap = gcnew Bitmap(In.cols, In.rows, In.step, System::Drawing::Imaging::PixelFormat::Format24bppRgb, ptr);
	// Declare a bitmap image and read the streame...
	BitmapImage^ image = gcnew BitmapImage();
	// Stream image data to stream...
	MemoryStream^ ms = gcnew MemoryStream();

	bitmap->Save(ms, System::Drawing::Imaging::ImageFormat::Bmp);
	ms->Seek(0, SeekOrigin::Begin);

	// Write stream to ImageSource...
	image->BeginInit();
	image->StreamSource = ms;
	image->EndInit();

	// gc(Unmanaged)
	// ms->Close();

	// gc(Managed)
	// delete(bitmap);
	// delete(ms);

	return image;
}
