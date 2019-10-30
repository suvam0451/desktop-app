#pragma once

#include "ExcelHelper.h"
#include <string>
#include <opencv2/opencv.hpp>

using namespace System;
using namespace System::Runtime::InteropServices;

// Media handling...
using namespace System::IO;
using namespace System::Drawing;
using namespace System::Windows::Media;
using namespace System::Windows::Media::Imaging;

class Blob {
public:
	std::vector<cv::Point> currentContour;
	cv::Rect currentBoundingRect;

	std::vector<cv::Point> centerPositions;

	double dblCurrentDiagonalSize;
	double dblCurrentAspectRatio;

	bool blnCurrentMatchFoundOrNewBlob;
	bool blnStillBeingTracked;
	int intNumOfConsecutiveFramesWithoutAMatch;

	cv::Point predictedNextPosition;

	Blob(std::vector<cv::Point> _contour);
	void predictNextPosition(void);
};

struct TextureCombiner_Preset {
	// Various nodes of the API  specify which textures should be looked up...
	
	// Albedo and Normal maps almost always used...
	bool UseAlbedo = true;
	bool UseNormal = true;

	// Other optional channels. Work as per presets...
	// Usually goes in one texture (Bottom Right)...
	int Size_01 = 1;
	bool UseAmbientOcclusion = false;
	bool UseMetalllic = false;
	bool UseRoughness = false;

	// Usually go in one texture (Bottom left)...
	// Size specifies how many textures are combined...
	int Size_02 = 1;
	bool UseHeight = false;
	bool UseTransparency = false;
	bool UseMetallic = false;
};

// Helper libarry for conversions between interop boundary...
class HelperLibrary
{
public:
	static std::string StringManagedToSTL(String^ In);
	static void DummyFunction(String^ _In);

	static BitmapImage^ GetImageSourceFromCV(cv::Mat& In);
};

class LaneDetector {
private:
	double img_size;
	double img_center;
	bool left_flag = false, right_flag = false;
	cv::Point right_b;  // Members of both line equations of the line boundaries
	double right_m;  // y = m*b + b
	cv::Point left_b;  //
	double left_m;  //

public:
	static cv::Mat deNoise(cv::Mat inputImage);  // Apply gaussian blur to the input image
	static cv::Mat edgeDetector(cv::Mat img_noise);  // Filter the image to obtain only edges
};
