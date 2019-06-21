using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ZXingSample.Droid
{
	[Activity(Label = "ZXingSample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			// Request external storage read and write permissions
			if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, Android.App.Application.Context.PackageName) != Permission.Granted
			&& PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, Android.App.Application.Context.PackageName) != Permission.Granted
			&& PackageManager.CheckPermission(Manifest.Permission.Internet, Android.App.Application.Context.PackageName) != Permission.Granted)
			{
				var permissions = new string[] { Manifest.Permission.Internet, Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
				RequestPermissions(permissions, 1);
			}

			base.OnCreate(savedInstanceState);
			ZXing.Net.Mobile.Forms.Android.Platform.Init();
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			LoadApplication(new App());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}