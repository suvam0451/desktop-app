#include "pch.h"

#include "daedalus-clr.h"

// using namespace cv;

daedalus_clr::TextureCombine_Type1_Backend::~TextureCombine_Type1_Backend()
{
	// Delete heap allocated resources...
	FileData->Clear();
	delete(_consoleref);
	delete(imgsrc);
	// Call post-delete...
	this->!TextureCombine_Type1_Backend();
}

void daedalus_clr::TextureCombine_Type1_Backend::HandleMediaDrop(String^ strarr)
{
	/*
	*	OpenCV Initialization
	*/
	std::string videoPath = HelperLibrary::StringManagedToSTL(strarr);
	cv::VideoCapture cap;
	cv::Mat imgFrame1;
	cv::Mat imgFrame2;
	std::vector<Blob> blobs;

	cv::Point crossingLine[2];
	char chCheckForEscKey = 0;

	cap.open(videoPath);

	CVColors

		if (!cap.isOpened()) {
			std::cout << "\nerror reading video file" << std::endl << std::endl;      // show error message
			return;
		}

	if (cap.get(cv::CAP_PROP_FRAME_COUNT) < 2) {
		std::cout << "\nerror: video file must have at least two frames";
		return;
	}


	// cap.read(imgFrame2);
	cap.read(imgFrame1);

	int intHorizontalLinePosition = (int)std::round((double)imgFrame1.rows * 0.35);
	crossingLine[0].x = 0;
	crossingLine[0].y = intHorizontalLinePosition;

	crossingLine[1].x = imgFrame1.cols - 1;
	crossingLine[1].y = intHorizontalLinePosition;

	char chCheckForKey = 0;
	bool bInFirstFrame = true;
	int frameCount = 2;
	int carCount = 0;

	/*
	*	OPENCV video loop
	*/
	while (cap.isOpened() && chCheckForEscKey != 27)
	{
		cap >> imgFrame1; // Alternatively cap.read()
		cap >> imgFrame2;

		cv::resize(imgFrame1, imgFrame1, cv::Size(1366, 768));
		cv::resize(imgFrame2, imgFrame2, cv::Size(1366, 768));
		cap.get(cv::CAP_PROP_FPS).ToString();

		std::vector<Blob> currentFrameBlobs;
		cv::Mat imgFrame1Copy = imgFrame1.clone();
		cv::Mat imgFrame2Copy = imgFrame2.clone();

		cv::Mat imgDifference;
		cv::Mat imgThresh;

		cv::cvtColor(imgFrame1Copy, imgFrame1Copy, cv::COLOR_BGR2GRAY);
		cv::cvtColor(imgFrame2Copy, imgFrame2Copy, cv::COLOR_BGR2GRAY);

		cv::GaussianBlur(imgFrame1Copy, imgFrame1Copy, cv::Size(5, 5), 0);
		cv::GaussianBlur(imgFrame2Copy, imgFrame2Copy, cv::Size(5, 5), 0);

		cv::absdiff(imgFrame1Copy, imgFrame2Copy, imgDifference);

		cv::threshold(imgDifference, imgThresh, 30, 255, cv::THRESH_BINARY);

		// cv::imshow("imgThresh", imgThresh);

		cv::Mat structuringElement3x3 = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(3, 3));
		cv::Mat structuringElement5x5 = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(5, 5));
		cv::Mat structuringElement7x7 = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(7, 7));
		cv::Mat structuringElement9x9 = cv::getStructuringElement(cv::MORPH_RECT, cv::Size(9, 9));

		for (int i = 0; i < 2; i++) {
			cv::dilate(imgThresh, imgThresh, structuringElement5x5);
			cv::dilate(imgThresh, imgThresh, structuringElement5x5);
			cv::erode(imgThresh, imgThresh, structuringElement5x5);
		}

		cv::Mat imgThreshCopy = imgThresh.clone();

		std::vector<std::vector<cv::Point>> contours;

		cv::findContours(imgThreshCopy, contours, cv::RETR_EXTERNAL, cv::CHAIN_APPROX_SIMPLE);

		drawAndShowContours(imgThresh.size(), contours, "imgContours");

		std::vector<std::vector<cv::Point> > convexHulls(contours.size());
		for (unsigned int i = 0; i < contours.size(); i++) {
			cv::convexHull(contours[i], convexHulls[i]);
		}

		drawAndShowContours(imgThresh.size(), convexHulls, "imgConvexHulls");

		for (auto& convexHull : convexHulls) {
			Blob possibleBlob(convexHull);

			if (possibleBlob.currentBoundingRect.area() > 400 &&
				possibleBlob.dblCurrentAspectRatio > 0.2 &&
				possibleBlob.dblCurrentAspectRatio < 4.0 &&
				possibleBlob.currentBoundingRect.width > 30 &&
				possibleBlob.currentBoundingRect.height > 30 &&
				possibleBlob.dblCurrentDiagonalSize > 60.0 &&
				(cv::contourArea(possibleBlob.currentContour) / (double)possibleBlob.currentBoundingRect.area()) > 0.50) {
				currentFrameBlobs.push_back(possibleBlob);
			}
		}

		drawAndShowContours(imgThresh.size(), currentFrameBlobs, "imgCurrentFrameBlobs");

		if (bInFirstFrame == true) {
			for (auto& currentFrameBlob : currentFrameBlobs) {
				blobs.push_back(currentFrameBlob);
			}
		}
		else {
			matchCurrentFrameBlobsToExistingBlobs(blobs, currentFrameBlobs);
		}

		drawAndShowContours(imgThresh.size(), blobs, "imgBlobs");
		imgFrame2Copy = imgFrame2.clone();          // get another copy of frame 2 since we changed the previous frame 2 copy in the processing above
		drawBlobInfoOnImage(blobs, imgFrame2Copy);

		bool blnAtLeastOneBlobCrossedTheLine = checkIfBlobsCrossedTheLine(blobs, intHorizontalLinePosition, carCount);

		if (blnAtLeastOneBlobCrossedTheLine == true) {
			cv::line(imgFrame2Copy, crossingLine[0], crossingLine[1], SCALAR_GREEN, 2);
		}
		else {
			cv::line(imgFrame2Copy, crossingLine[0], crossingLine[1], SCALAR_RED, 2);
		}

		drawCarCountOnImage(carCount, imgFrame2Copy);

		// Resulting image output
		cv::imshow("imgFrame2Copy", imgFrame2Copy);

		currentFrameBlobs.clear();
		imgFrame1 = imgFrame2.clone();           // move frame 1 up to where frame 2 is

												 // End of break...
		if ((cap.get(cv::CAP_PROP_POS_FRAMES) + 1) < cap.get(cv::CAP_PROP_FRAME_COUNT)) {
			cap.read(imgFrame2);
		}
		else {
			std::cout << "end of video\n";
			break;
		}

		bInFirstFrame = false;
		frameCount++;
		
		chCheckForEscKey = cv::waitKey(1);

		if (chCheckForEscKey != 27) {
			cv::waitKey(0);
		}
	}
	cap.release();
	cv::destroyAllWindows();
}

void daedalus_clr::TextureCombine_Type1_Backend::matchCurrentFrameBlobsToExistingBlobs(std::vector<Blob>& existingBlobs, std::vector<Blob>& currentFrameBlobs)
{
	for (auto& existingBlob : existingBlobs) {
		existingBlob.blnCurrentMatchFoundOrNewBlob = false;
		existingBlob.predictNextPosition();
	}

	for (auto& currentFrameBlob : currentFrameBlobs) {
		int intIndexOfLeastDistance = 0;
		double dblLeastDistance = 100000.0;

		for (unsigned int i = 0; i < existingBlobs.size(); i++) {

			if (existingBlobs[i].blnStillBeingTracked == true) {

				double dblDistance = distanceBetweenPoints(currentFrameBlob.centerPositions.back(), existingBlobs[i].predictedNextPosition);

				if (dblDistance < dblLeastDistance) {
					dblLeastDistance = dblDistance;
					intIndexOfLeastDistance = i;
				}
			}
		}

		if (dblLeastDistance < currentFrameBlob.dblCurrentDiagonalSize * 0.5) {
			addBlobToExistingBlobs(currentFrameBlob, existingBlobs, intIndexOfLeastDistance);
		}
		else {
			addNewBlob(currentFrameBlob, existingBlobs);
		}
	}

	for (auto& existingBlob : existingBlobs) {

		if (existingBlob.blnCurrentMatchFoundOrNewBlob == false) {
			existingBlob.intNumOfConsecutiveFramesWithoutAMatch++;
		}

		if (existingBlob.intNumOfConsecutiveFramesWithoutAMatch >= 5) {
			existingBlob.blnStillBeingTracked = false;
		}
	}
}

void daedalus_clr::TextureCombine_Type1_Backend::addBlobToExistingBlobs(Blob& currentFrameBlob, std::vector<Blob>& existingBlobs, int& intIndex){
	existingBlobs[intIndex].currentContour = currentFrameBlob.currentContour;
	existingBlobs[intIndex].currentBoundingRect = currentFrameBlob.currentBoundingRect;
	existingBlobs[intIndex].centerPositions.push_back(currentFrameBlob.centerPositions.back());
	existingBlobs[intIndex].dblCurrentDiagonalSize = currentFrameBlob.dblCurrentDiagonalSize;
	existingBlobs[intIndex].dblCurrentAspectRatio = currentFrameBlob.dblCurrentAspectRatio;
	existingBlobs[intIndex].blnStillBeingTracked = true;
	existingBlobs[intIndex].blnCurrentMatchFoundOrNewBlob = true;
}

void daedalus_clr::TextureCombine_Type1_Backend::addNewBlob(Blob& currentFrameBlob, std::vector<Blob>& existingBlobs){
	currentFrameBlob.blnCurrentMatchFoundOrNewBlob = true;
	existingBlobs.push_back(currentFrameBlob);
}

double daedalus_clr::TextureCombine_Type1_Backend::distanceBetweenPoints(cv::Point point1, cv::Point point2){
	int intX = abs(point1.x - point2.x);
	int intY = abs(point1.y - point2.y);
	return(sqrt(pow(intX, 2) + pow(intY, 2)));
}

void daedalus_clr::TextureCombine_Type1_Backend::drawAndShowContours(cv::Size imageSize, std::vector<std::vector<cv::Point>> contours, std::string strImageName) {
	CVColors
		cv::Mat image(imageSize, CV_8UC3, SCALAR_BLACK);
	cv::drawContours(image, contours, -1, SCALAR_WHITE, -1);
	// cv::imshow(strImageName, image);
}

void daedalus_clr::TextureCombine_Type1_Backend::drawAndShowContours(cv::Size imageSize, std::vector<Blob> blobs, std::string strImageName) {
	CVColors
	cv::Mat image(imageSize, CV_8UC3, SCALAR_BLACK);

	std::vector<std::vector<cv::Point> > contours;

	for (auto& blob : blobs) {
		if (blob.blnStillBeingTracked == true) {
			contours.push_back(blob.currentContour);
		}
	}

	cv::drawContours(image, contours, -1, SCALAR_WHITE, -1);

	// cv::imshow(strImageName, image);
}

bool daedalus_clr::TextureCombine_Type1_Backend::checkIfBlobsCrossedTheLine(std::vector<Blob>& blobs, int& intHorizontalLinePosition, int& carCount){
	bool blnAtLeastOneBlobCrossedTheLine = false;
	for (auto blob : blobs) {
		if (blob.blnStillBeingTracked == true && blob.centerPositions.size() >= 2) {
			int prevFrameIndex = (int)blob.centerPositions.size() - 2;
			int currFrameIndex = (int)blob.centerPositions.size() - 1;

			if (blob.centerPositions[prevFrameIndex].y > intHorizontalLinePosition&& blob.centerPositions[currFrameIndex].y <= intHorizontalLinePosition) {
				carCount++;
				blnAtLeastOneBlobCrossedTheLine = true;
			}
		}
	}
	return blnAtLeastOneBlobCrossedTheLine;
}

void daedalus_clr::TextureCombine_Type1_Backend::drawBlobInfoOnImage(std::vector<Blob>& blobs, cv::Mat& imgFrame2Copy){
	CVColors

	for (unsigned int i = 0; i < blobs.size(); i++) {
		if (blobs[i].blnStillBeingTracked == true) {
			cv::rectangle(imgFrame2Copy, blobs[i].currentBoundingRect, SCALAR_RED, 2);

			int intFontFace = cv::FONT_HERSHEY_COMPLEX;
			
			double dblFontScale = blobs[i].dblCurrentDiagonalSize / 60.0;
			int intFontThickness = (int)std::round(dblFontScale * 1.0);

			cv::putText(imgFrame2Copy, std::to_string(i), blobs[i].centerPositions.back(), intFontFace, dblFontScale, SCALAR_GREEN, intFontThickness);
		}
	}
}

void daedalus_clr::TextureCombine_Type1_Backend::drawCarCountOnImage(int& carCount, cv::Mat& imgFrame2Copy){
	CVColors

	int intFontFace = cv::FONT_HERSHEY_COMPLEX;
	double dblFontScale = (imgFrame2Copy.rows * imgFrame2Copy.cols) / 300000.0;
	int intFontThickness = (int)std::round(dblFontScale * 1.5);

	cv::Size textSize = cv::getTextSize(std::to_string(carCount), intFontFace, dblFontScale, intFontThickness, 0);

	cv::Point ptTextBottomLeftPosition;

	ptTextBottomLeftPosition.x = imgFrame2Copy.cols - 1 - (int)((double)textSize.width * 1.25);
	ptTextBottomLeftPosition.y = (int)((double)textSize.height * 1.25);

	cv::putText(imgFrame2Copy, std::to_string(carCount), ptTextBottomLeftPosition, intFontFace, dblFontScale, SCALAR_GREEN, intFontThickness);

}

void daedalus_clr::TextureCombine_Type1_Backend::HandleFileDrop(ImageDelegate^ imagedel, array<String^>^ strarr)
{
	FileData->Clear();

	for each (String ^ it in strarr)
	{
		Regex^ rx;
		MatchCollection^ matches;

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
	}

	String^ res;
	// Allocate matrices. Fallback to defaulf if not caught...
	cv::Mat albedo = (FileData->TryGetValue("albedo", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
	cv::Mat normal = (FileData->TryGetValue("normal", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
	cv::Mat height = (FileData->TryGetValue("height", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
	cv::Mat rough = (FileData->TryGetValue("rough", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
	cv::Mat metal = (FileData->TryGetValue("metal", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));
	cv::Mat ao = (FileData->TryGetValue("ao", res)) ? cv::imread(HelperLibrary::StringManagedToSTL(res)) : cv::Mat(cv::Size(_size, _size), CV_64FC1, cv::Scalar(0));

	cv::Mat tmp1, tmp2, out, combined_01, combined_02;
	cv::Mat* tmparr = new cv::Mat[3];
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

	cv::hconcat(albedo, normal, tmp1);
	cv::hconcat(combined_01, combined_02, tmp2);

	// Vertical concatenation to form 4 quadrants
	cv::vconcat(tmp1, tmp2, out);

	imgsrc = HelperLibrary::GetImageSourceFromCV(out);

	// MessageBox::Show("Used to meet in the eastside");
	imagedel(imgsrc);
	// consoleDel(gcnew String("Completed Task"));

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
