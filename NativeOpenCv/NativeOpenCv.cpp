// NativeOpenCv.cpp
#include "NativeOpenCv.h"
#include <opencv2/core.hpp>
#include <opencv2/highgui.hpp>

#define DllExport extern "C"

DllExport double GetMatMeanY(int black_length, int white_length) {
	cv::Mat src1 = cv::Mat::zeros(100, black_length, CV_8UC1);
	cv::Mat src2(100, white_length, CV_8UC1, cv::Scalar(255, 255, 255));
	cv::Mat dst;

	hconcat(src1, src2, dst);

	return cv::mean(dst)[0];
}
