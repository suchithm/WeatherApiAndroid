using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Newtonsoft.Json;
//http://l.yimg.com/a/i/us/we/52/28.gif

//http://l.yimg.com/a/i/us/we/52/30.gif

//select * from weather.forecast where woeid in (select woeid from geo.places(1) where text="bangalore") and u='c'

//https://query.yahooapis.com/v1/public/yql?q=qry&format=json

//https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text="bangalore") and u='c'&format=json 
using System.Threading.Tasks;
using System.Net;
using Android.Views.InputMethods;
using Android.Graphics;


namespace WeatherInfo
{
	[Activity ( Label = "Weather Condition" , MainLauncher = true , Icon = "@drawable/icon" )]
	public class MainActivity : Activity
	{ 
		ImageView imgCondition;
		TextView lblTemp;
		TextView lblHumidity;
		TextView lblWindSpeed;
		TextView lblConditionText;
		TextView lblSunrise;
		TextView lblSunSet;
		ListView ListViewForeCast;
		EditText txtLocation;
		WeatherClass objYahooWeatherClass;
		
		const string strYahooApi=  "https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text=\"bangalore\") and u='c'&format=json";
		protected override void OnCreate ( Bundle bundle )
		{
			base.OnCreate ( bundle ); 
			// Set our view from the "main" layout resource
			SetContentView ( Resource.Layout.Main ); 
			InitializeControl ();
	
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
					BindData();
				}
			}; 
				
			 
		}
		void BindData()
		{
			lblTemp.Text = string.Format ( "Temperature :{0}{1}" , objYahooWeatherClass.query.results.channel.item.condition.temp , objYahooWeatherClass.query.results.channel.units.temperature );
			lblHumidity.Text = string.Format ( "Humidity :{0}{1}" , objYahooWeatherClass.query.results.channel.atmosphere.humidity,"%" );
			lblWindSpeed.Text=string.Format ( "Wind Speed :{0}{1}" , objYahooWeatherClass.query.results.channel.wind.speed,objYahooWeatherClass.query.results.channel.units.speed );
			lblConditionText.Text=string.Format ( " {0}" , objYahooWeatherClass.query.results.channel.item.condition.text );
			lblSunrise.Text =string.Format("Sunrise :{0}",objYahooWeatherClass.query.results.channel.astronomy.sunrise);
			lblSunSet.Text =string.Format("Sunset :{0}",objYahooWeatherClass.query.results.channel.astronomy.sunset);
	 
		}
		void InitializeControl()
		{
			imgCondition = FindViewById<ImageView> ( Resource.Id.imgWeatherIcon ); 
			txtLocation = FindViewById<EditText> ( Resource.Id.txtSearch );
			lblTemp = FindViewById<TextView> ( Resource.Id.lblTempWC );
			lblHumidity=FindViewById<TextView> ( Resource.Id.lblHumidity );
			lblWindSpeed=FindViewById<TextView> ( Resource.Id.lblWindSpeed );
			lblConditionText=FindViewById<TextView> ( Resource.Id.lblConditionText );
			lblSunrise=FindViewById<TextView> ( Resource.Id.lblSunrise );
			lblSunSet=FindViewById<TextView> ( Resource.Id.lblSunset ); 
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


