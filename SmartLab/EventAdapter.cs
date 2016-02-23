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
	public class EventAdapter : BaseAdapter<Event>
	{
		private Activity context;
		public List<Event> Events = new List<Event>(); 

		public EventAdapter (Activity context, List<Event> ev)
		{
			this.context = context;
			this.Events = ev;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);


			view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = this.Events [position].Title;
			view.FindViewById<TextView> (Android.Resource.Id.Text2).Text = this.Events [position].Date.Hour == 0 ? this.Events [position].Date.ToString ("D") : this.Events [position].Date.ToString ("f"); 
			return view;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count {
			get {
				return this.Events.Count;
			}
		}

		public override Event this [int index] {
			get {
				return this.Events [index];
			}
		}
	}
}

