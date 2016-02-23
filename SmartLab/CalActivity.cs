
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace SmartLab
{
	[Activity (Label = "Calendar", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/SmartLabTheme")]
	public class CalActivity : ListActivity
	{
		public List<Event> Calendar = new List<Event>();
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			this.Calendar = Cache.GetEvents ();
			this.ListAdapter = new EventAdapter (this, this.Calendar);
			ThreadPool.QueueUserWorkItem (p => UpdateCalendar ());
		}

		async private void UpdateCalendar()
		{
			var cal = await Api.GetCalendar();
			if (cal.Count > 0) {
				this.Calendar = cal;
				Cache.SetEvents (cal);
				RunOnUiThread (() => {
					this.ListAdapter = new EventAdapter (this, this.Calendar);
				});
			}
		}
	}
}

