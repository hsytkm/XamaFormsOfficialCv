// NativeOpenCv.cpp
#include "NativeOpenCv.h"
#include <opencv2/core.hpp>
#define DllExport extern "C"

DllExport double GetMatMeanY(int black_length, int white_length) {
	int row = 100;

	// 1. 引数で指定された割合で、単色の黒/白 Mat を作成
	cv::Mat brack = cv::Mat::zeros(row, black_length, CV_8UC1);
	cv::Mat white(row, white_length, CV_8UC1, cv::Scalar(255, 255, 255));

	// 2. サイドバイサイドで 2つのMat を結合
	cv::Mat merge;
	hconcat(brack, white, merge);

	// 3. 結合したMatの平均輝度値を求める（単色なので輝度(Y)と呼べない気もする）
	return cv::mean(merge)[0];
}
