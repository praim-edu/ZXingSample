using System.IO;
using ZXingSample.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Files_Android))]
namespace ZXingSample.Droid
{
	public class Files_Android : Files
	{
		public void Save(string filename, string filecontent)
		{
			bool isReadonly = Android.OS.Environment.MediaMountedReadOnly.Equals(Android.OS.Environment.ExternalStorageState);
			bool isWriteable = Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState);
			// check if external storage is writable, write test file into /storage/emulated/0/Documents
			if (isWriteable)
			{
				var dir = Android.OS.Environment.ExternalStorageDirectory;
				var backingFile = Path.Combine(dir.AbsolutePath, "Documents", filename);
				using (var writer = File.CreateText(backingFile))
				{
					writer.WriteLine(filecontent);
				}
			}
		}
	}
}