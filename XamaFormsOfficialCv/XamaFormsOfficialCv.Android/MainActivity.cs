using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace XamaFormsOfficialCv.Droid
{
    [Activity(Label = "XamaFormsOfficialCv", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        // ◆追加：ここから１
        [System.Runtime.InteropServices.DllImport("NativeOpenCv")]
        private static extern double GetMatMeanY(int black_length, int white_length);
        // ◆追加：ここまで１

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //◆追加：ここから２
            // 黒画像と白画像の割合を指定して、平均輝度値を求める
            var y0 = GetMatMeanY(1, 1);     // 255 * 1/2 = 127.5
            var y1 = GetMatMeanY(2, 1);     // 255 * 1/3 =  85.0
            var y2 = GetMatMeanY(200, 300); // 255 * 3/5 = 153.0
            // ◆追加：ここまで２
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}