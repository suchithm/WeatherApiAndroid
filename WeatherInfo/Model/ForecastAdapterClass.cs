using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using Android.Views;

namespace WeatherInfo
{
	public class ForecastAdapterClass : BaseAdapter
	{
		Activity context;
		List<string> lstCategory;
		internal event Action<string> CategoryClicked; 
		TextView lblCategory;
		public ForecastAdapterClass (Activity c,List<string> lstCategoryArg )
		{
			context = c;
			lstCategory = lstCategoryArg;
		}
		public override int Count {
			get { return lstCategory.Count; }
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}
		// create a new ImageView for each item referenced by the Adapter
		public override View GetView (int position, View convertView, ViewGroup parent)
		{ 
			View view = convertView;
			if ( view == null )
			{
				view = context.LayoutInflater.Inflate ( Resource.Layout.ForecastCustomLayout , parent , false );  
			}
 
			lblCategory = view.FindViewById<TextView> ( Resource.Id.lblCategoryCustom );
			lblCategory.Text = lstCategory [position].ToString();
			lblCategory.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;

			return view;
		}  
	}
	static class ViewHolderClass
	{
		TextView txtDayDate;
		ImageView imgWeather;
		TextView txtTextCondition;
		TextView txtHigh;
		TextView txtLow;
	}
}

