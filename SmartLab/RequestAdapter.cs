using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;

namespace SmartLab
{
	public class RequestAdapter : BaseAdapter<Request>
	{
		private Activity context;
		public Response Response; 
		public RequestAdapter (Activity context, Response r)
		{
			this.context = context;
			this.Response = r;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(position == this.Response.Requests.Count ? Android.Resource.Layout.SimpleListItem1 : Android.Resource.Layout.SimpleListItem2, null);

			if (position == this.Response.Requests.Count) {
				view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = String.Format ("{0} Requests, {1} In Use, {2} Idle & {3} Off.", this.Response.Help, this.Response.InUse, this.Response.Idle, this.Response.Offline);
				view.FindViewById<TextView> (Android.Resource.Id.Text1).TextAlignment = TextAlignment.Center;
				view.FindViewById<TextView> (Android.Resource.Id.Text1).SetTextColor (Color.Rgb(111, 38, 135));
			} else {
				view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = this.Response.Requests [position].Location;
				if (!this.Response.Requests [position].IsBizCalc ()) {
					view.FindViewById<TextView> (Android.Resource.Id.Text1).SetTextColor (Color.Red);
				} else {
					view.FindViewById<TextView> (Android.Resource.Id.Text1).SetTextColor (Color.Rgb(111, 38, 135));
				}
				view.FindViewById<TextView> (Android.Resource.Id.Text2).Text = this.Response.Requests [position].Created.ToString ("G");
			}
			return view;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count {
			get {
				return this.Response.Requests.Count+1;
			}
		}

		public override Request this [int index] {
			get {
				return this.Response.Requests [index];
			}
		}
	}
}

