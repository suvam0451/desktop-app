#include "pch.h"
#include "HelperLibrary.h"

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
