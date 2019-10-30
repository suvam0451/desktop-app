#pragma once
using namespace System::IO;
using namespace System;
using namespace System::Windows;

#include <OpenXLSX/OpenXLSX.h>


namespace daedalus_clr {
	public ref class ExcelHelper
	{
	public:
		static int UniformFactorMethod(System::String^ ExcelFilePath, bool CreateNewFile);
	};
}