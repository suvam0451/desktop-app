#pragma once

#include <string>
#include <opencv2/opencv.hpp>

using namespace System;
using namespace System::Runtime::InteropServices;

// Media handling...
using namespace System::IO;
using namespace System::Drawing;
using namespace System::Windows::Media;
using namespace System::Windows::Media::Imaging;

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
	static std::string StringManagedToSTL(String^ In)
	{
		const char* mine = (const char*)Marshal::StringToHGlobalAnsi(In).ToPointer();
		std::string retval = mine;
		Marshal::FreeHGlobal(IntPtr((void*)mine));
		//delete(mine);
		return retval;
	}

	// static String^ StringSTLToManaged(std::string In)
	// {
	// 	const char* mine = (const char*)Marshal::StringToHGlobalAnsi(In).ToPointer();
	// 	std::string retval = mine;
	// 	Marshal::FreeHGlobal(IntPtr((void*)mine));
	// 	//delete(mine);
	// 	return String^();
	// }

	//static ImageSource^ GetImageSourceFromCV(cv::Mat &In) {
	static BitmapImage^ GetImageSourceFromCV(cv::Mat& In) {
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

	//static void TextureCombiner_Parse(Dictionary<String^, String^>^ &FileData, ) {
		
	//}
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
