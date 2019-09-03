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
	static std::string StringManagedToSTL(String^ In) {
		// Allocate handle...
		const char* mine = (const char*)Marshal::StringToHGlobalAnsi(In).ToPointer();
		// Allocate memory reference
		std::string retval = mine;
		// Free allocated memory
		Marshal::FreeHGlobal(IntPtr((void*)mine));
		//delete(mine);
		// Return stl string
		return retval;
	}

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

		// gc
		//ms->Close();
		//ms->Dispose();
		//ImageSource^ retval = image;

		// gc
		// Managed
		//delete(bitmap);
		//delete(ms);

		// return ImageSource...
		return image;
		//return image;
	}

	//static void TextureCombiner_Parse(Dictionary<String^, String^>^ &FileData, ) {
		
	//}
};

