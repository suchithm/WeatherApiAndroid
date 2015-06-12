using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Newtonsoft.Json;
//http://l.yimg.com/a/i/us/we/52/28.gif

//select * from weather.forecast where woeid in (select woeid from geo.places(1) where text="bangalore") and u='c'

//https://query.yahooapis.com/v1/public/yql?q=qry&format=json

//https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text="bangalore") and u='c'&format=json 
using System.Threading.Tasks;
using System.Net;
using Android.Views.InputMethods;


namespace WeatherInfo
{
	[Activity ( Label = "WeatherInfo" , MainLauncher = true , Icon = "@drawable/icon" )]
	public class MainActivity : Activity
	{ 
		const string strYahooApi=  "https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text=\"bangalore\") and u='c'&format=json";
		protected override void OnCreate ( Bundle bundle )
		{
			base.OnCreate ( bundle );
			WeatherClass objYahooWeatherClass;
			// Set our view from the "main" layout resource
			SetContentView ( Resource.Layout.Main ); 
			EditText txtLocation = FindViewById<EditText> ( Resource.Id.txtSearch );
			
			txtLocation.KeyPress +=async delegate(object sender, View.KeyEventArgs e) {
				InputMethodManager inputManager = (InputMethodManager)this.GetSystemService (Context.InputMethodService); 
				inputManager.HideSoftInputFromWindow (this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways); 
				if ( e.KeyCode == Keycode.Enter && e.Event.Action == KeyEventActions.Down )
				{
					string strWeatherJson = await fnDownloadString(strYahooApi);
					objYahooWeatherClass = JsonConvert.DeserializeObject<WeatherClass>(strWeatherJson);

					Console.WriteLine(" date "+ objYahooWeatherClass.query.results.channel.item.forecast[0].date);
					Console.WriteLine(" day "+ objYahooWeatherClass.query.results.channel.item.forecast[0].day);
					foreach(Forecast forecast in objYahooWeatherClass.query.results.channel.item.forecast)
					{
						Console.WriteLine(" code "+ forecast.code);
						Console.WriteLine(" date "+ forecast.date);
						Console.WriteLine(" day "+forecast.day);
					}
				}
			}; 
				
			 
		}
		async Task<string> fnDownloadString(string strUri)
		{ 
			WebClient webclient = new WebClient ();
			string strResultData;
			try
			{
				strResultData= await webclient.DownloadStringTaskAsync (new Uri(strUri));
				Console.WriteLine(strResultData);
			}
			catch
			{
				strResultData = "Exception";
				RunOnUiThread ( () =>
				{ 
					Toast.MakeText ( this , "Unable to connect to server!!!" , ToastLength.Short ).Show ();
				} );
			}
			finally
			{
				webclient.Dispose ();
				webclient = null; 
			}

			return strResultData;
		}

	}
}


