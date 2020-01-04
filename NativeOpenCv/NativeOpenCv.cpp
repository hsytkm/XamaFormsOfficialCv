// NativeOpenCv.cpp
#include "NativeOpenCv.h"
#include <opencv2/core.hpp>
#define DllExport extern "C"

DllExport double GetMatMeanY(int black_length, int white_length) {
	int row = 100;

	// 1. �����Ŏw�肳�ꂽ�����ŁA�P�F�̍�/�� Mat ���쐬
	cv::Mat brack = cv::Mat::zeros(row, black_length, CV_8UC1);
	cv::Mat white(row, white_length, CV_8UC1, cv::Scalar(255, 255, 255));

	// 2. �T�C�h�o�C�T�C�h�� 2��Mat ������
	cv::Mat merge;
	hconcat(brack, white, merge);

	// 3. ��������Mat�̕��ϋP�x�l�����߂�i�P�F�Ȃ̂ŋP�x(Y)�ƌĂׂȂ��C������j
	return cv::mean(merge)[0];
}
