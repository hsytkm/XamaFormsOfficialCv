# Xamarin.Forms から OpenCV for Android を利用する



## はじめに

OpenCV公式が公開している Android用ライブラリを利用して、Xamarin.Forms(Android) から C/C++ で OpenCV を操作する方法をまとめました。

今回は動作確認用のサンプルとして、単色の黒/白 2つのMat画像を結合して平均輝度値を求めています。

以降は arm64-v8a に絞って書いていますので、他のアーキテクチャの方は適宜読み替えてください。



## 確認環境

- OpenCV 4.2.0

- Windows 10
- Visual Studio Community 2019 16.4.2
- Xamarin.Forms 4.4.0.991265
- Google Pixel 3 (Android 10.0 - API 29)



## セットアップ

特に変わったことはしていません。



<details><summary>VisualStudioプロジェクト作成</summary><div>

### Xamarin.Formsプロジェクト

モバイルアプリ(Xamarin.Forms) から、"XamaFormsOfficialCv" として作成しました。

![XamarinProject](C:\Users\t_hos\Desktop\冬宿題\official_opencv\xamarin_project.png "XamarinProject")



### NativeLibraryプロジェクト

ダイナミック共有ライブラリ(Android) から、"NativeOpenCv" として作成しました。

![NativeProject](C:\Users\t_hos\Desktop\冬宿題\official_opencv\native_project.png "NativeProject")



先に作成したXamaFormsOfficialCv.Android プロジェクトの [参照の追加] より NativeOpenCv を参照します。

この時点で一旦、動作確認しておくと安全です。

</div></details>



### OpenCVライブラリの取得

[OpenCVの公式](https://opencv.org/releases/) から取得して、良い感じの場所に展開しておきます。

今回は "C:\opencv\OpenCV-android-sdk" に置きました。

![OpenCvRelase](C:\Users\t_hos\Desktop\冬宿題\official_opencv\opencv_release.png "OpenCvRelase")



## 自作ライブラリの対応

### STL設定

デフォルトでは [LLVM libc++ スタティックライブラリ(c++\_static)] になっていたので、[共有ライブラリ (c++\_shared)] に変えました。



![STLSetting](C:\Users\t_hos\Desktop\冬宿題\official_opencv\lib_setting_stl.png "STLSetting")



### OpenCVインクルード

展開したOpenCVのヘッダフォルダをインクルードします。

 ``` 
C:\opencv\OpenCV-android-sdk\sdk\native\jni\include
 ```

![IncludeOpenCV](C:\Users\t_hos\Desktop\冬宿題\official_opencv\include_opencv.png "IncludeOpenCV")



### OpenCVライブラリ

展開したOpenCVの各ライブラリフォルダをインクルードします。

 ``` 
C:\opencv\OpenCV-android-sdk\sdk\native\libs\arm64-v8a
C:\opencv\OpenCV-android-sdk\sdk\native\3rdparty\libs\arm64-v8a
C:\opencv\OpenCV-android-sdk\sdk\native\staticlibs\arm64-v8a
 ```

![AddOpenCVLibPath](C:\Users\t_hos\Desktop\冬宿題\official_opencv\add_opencv.png "AddOpenCVLibPath")



### OpenCVライブラリ名

上記ディレクトリ内の各ライブラリ名を指定します。今回は全てのライブラリを列挙してみました。

ライブラリ名には、libXXXX.a の XXXX の部分のみを記述するようにして下さい。

ちなみに libXXXX.a とフルで書くと、LINK で "cannot find" になります。~~VSが気を利かせてくれればハマらずに済んだのですが…~~

``` 
opencv_java4
cpufeatures
IlmImf
ittnotify
libjasper
libjpeg-turbo
libpng
libprotobuf
libtiff
libwebp
quirc
tbb
tegra_hal
opencv_calib3d
opencv_core
opencv_dnn
opencv_features2d
opencv_flann
opencv_highgui
opencv_imgcodecs
opencv_imgproc
opencv_ml
opencv_objdetect
opencv_photo
opencv_stitching
opencv_video
opencv_videoio
```

![AddOpenCVLibName](C:\Users\t_hos\Desktop\冬宿題\official_opencv\add_opencv_lib.png "AddOpenCVLibName")



### ソースコード(C++)

デフォルトで作成される [プロジェクト名.h] と  [プロジェクト名.cpp]  の中身は削除して、以下を追加しました。

サンプルなので ややこしいことはしていません。

``` C++
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
```



自作ライブラリの対応は以上です。



## Xamarin(Android)の対応

### OpenCVライブラリ

Android側のプロジェクトにフォルダを作成して、OpenCVの共有ライブラリ(*.so) を追加します。

- プロジェクトに作成したフォルダ

  ```
  [Project]\libs\arm64-v8a
  ```

- 追加した共有ライブラリファイル

  C:\opencv\OpenCV-android-sdk\sdk\native\libs\arm64-v8a 以下の so ファイル（1つだけでした）

  ```
  C:\opencv\OpenCV-android-sdk\sdk\native\libs\arm64-v8a\libopencv_java4.so
  ```

![LibPath](C:\Users\t_hos\Desktop\冬宿題\official_opencv\lib_path.png "LibPath")

追加したライブラリ(*.so) のプロパティを変更します。

- ビルドアクション：AndroidNativeLirary
- 出力ディレクトリにコピー：新しい場合はコピーする

![LibSetting](C:\Users\t_hos\Desktop\冬宿題\official_opencv\lib_setting.png "LibSetting")



### ソースコード(C#)

とりあえず動作を見るだけなので、デフォルトで作成される MainActivity.cs に自作ライブラリの呼び出し処理（P/Invoke）を追加しました。

```C#
public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
{
    // ◆追加：ここから１
    [System.Runtime.InteropServices.DllImport("NativeOpenCv")]
    private static extern double GetMatMeanY(int black_length, int white_length);
    // ◆追加：ここまで１

    protected override void OnCreate(Bundle savedInstanceState)
    {
        ～～割愛～～

        // ◆追加：ここから２
        // 黒画像と白画像の割合を指定して、平均輝度値を求める
        var y0 = GetMatMeanY(1, 1);     // 255 * 1/2 = 127.5
        var y1 = GetMatMeanY(2, 1);     // 255 * 1/3 =  85.0
        var y2 = GetMatMeanY(200, 300); // 255 * 3/5 = 153.0
        // ◆追加：ここまで２
    }
    ～～割愛～～
}
```



対応は以上です。

実行しても何も分かりませんが、ブレークポイントを仕掛ければコメント通りの輝度値が取得できるはずです。



## サンプル

本紹介で使用したリポジトリは以下で公開しています。

https://github.com/hsytkm/XamaFormsOfficialCv



## 参考

[OpenCV Android](https://opencv.org/android/)

[Android C++ ライブラリ サポート](https://developer.android.com/ndk/guides/cpp-support?hl=ja)

[Android プロジェクトへの C / C++ コードの追加](https://developer.android.com/studio/projects/add-native-code?hl=ja)



## 終わりに

2019年冬休みの自分への課題を記事にまとめました。

本当はOpenCVのソースコードをセルフビルドして利用したかったのですが、CMakeやらLinkやらのエラーが取れないまま休みが終わってしましました…

仕事が始まるとバタバタして熱が冷めてしまいそうですが、時間を見つけてリベンジしたい！



