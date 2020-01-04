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
            var y0 = GetMatMeanY(100, 100);     // =127.5
            var y1 = GetMatMeanY(200, 100);     // = 85.0
            var y2 = GetMatMeanY(200, 300);     // =153.0
            // ◆追加：ここまで２
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}