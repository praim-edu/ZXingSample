using Newtonsoft.Json;
using System;
using System.Net.Http;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace ZXingSample
{
	public partial class FullScreenScanning : ZXingScannerPage
	{
		public FullScreenScanning()
		{
			InitializeComponent();
		}

		public void Handle_OnScanResult(Result result)
		{
			if (result.Text.Contains("https://api.cloud.praim.com/acme"))
			{
				Device.BeginInvokeOnMainThread(async () =>
				{
					DependencyService.Get<Toast>().Show("Collegamento in corso, Attendere...");
					HttpClient client = new HttpClient();
					var uri = new Uri(result.Text);
					HttpResponseMessage response = await client.GetAsync(uri);
					if (response.IsSuccessStatusCode)
					{
						var content = await response.Content.ReadAsStringAsync();
						var acme = JsonConvert.DeserializeObject<AcmeJSON>(content);
						if (acme.Status == "ok")
						{
							DependencyService.Get<Toast>().Show(
								"Azione richiesta: " + acme.Data.ActionRequested + Environment.NewLine +
								"Azione ottenuta: " + acme.Data.Pippo);

							DependencyService.Get<Files>().Save("test.txt", acme.Data.Pippo);

							XASmbServer xASmb = new XASmbServer("127.0.0.1", "test", "test");
							xASmb.Start();
							xASmb.Stop();
						}
					}

					//await DisplayAlert("Scanned result", result.Text, "OK");
				});
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			IsScanning = true;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			IsScanning = false;
		}
	}

	public class AcmeJSON
	{
		string status;
		public string Status { get => status; set => status = value; }

		AcmeData data;
		public AcmeData Data { get => data; set => data = value; }
	}

	public class AcmeData
	{
		string pippo;
		public string Pippo { get => pippo; set => pippo = value; }

		string actionRequested;
		public string ActionRequested { get => actionRequested; set => actionRequested = value; }
	}
}