#pragma once

#include <string>
#include <vector>

using namespace std;

class CT_T1_Core
{
private:
	vector<string> TexturesetData;

public:
	void UpdateRecords(vector<string>);
	void ResetRecords(vector<string>);
};

