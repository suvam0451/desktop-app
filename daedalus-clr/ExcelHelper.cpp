#include "pch.h"
#include "ExcelHelper.h"
#include "HelperLibrary.h"
using namespace OpenXLSX;
using namespace daedalus_clr;

int ExcelHelper::UniformFactorMethod(System::String^ ExcelFilePath, bool CreateNewFile)
{
	std::string unmanagedPath = HelperLibrary::StringManagedToSTL(ExcelFilePath);
	XLDocument doc(unmanagedPath);
	auto wks = doc.Workbook().Worksheet("Sheet1");
	auto _range = wks.Range();
	MessageBox::Show(_range.NumRows().ToString() + " " + _range.NumColumns().ToString());
	return 0;
}
