using Android.Widget;
using ZXingSample.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Toast_Android))]
namespace ZXingSample.Droid
{
	public class Toast_Android : Toast
	{
		public void Show(string message)
		{
			Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
		}
	}
}