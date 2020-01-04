// NativeOpenCv.cpp
#include "NativeOpenCv.h"

#include <opencv2/core.hpp>

#define DllExport extern "C"

DllExport double GetMatMeanY(int black_length, int white_length) {
	int row = 100;
	cv::Mat brack = cv::Mat::zeros(row, black_length, CV_8UC1);
	cv::Mat white(row, white_length, CV_8UC1, cv::Scalar(255, 255, 255));
	cv::Mat merge;

	hconcat(brack, white, merge);
	return cv::mean(merge)[0];
}
