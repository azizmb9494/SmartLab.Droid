using System;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Net;
using Android.Preferences;

namespace SmartLab
{
	[Activity (Label = "SMART Lab", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/SmartLabTheme")]
	public class MainActivity : ListActivity
	{
		private Timer _Timer { get; set; }
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			_Timer = new Timer (o => ThreadPool.QueueUserWorkItem (p => UpdateData ()), null, 0, 5000);
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Layout.menu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		private bool GetBizCalcOnly()
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences (this);
			return prefs.GetBoolean ("BizCalcOnly", false);
		}

		private bool GetHideBizCalc()
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences (this);
			return prefs.GetBoolean ("HidesBizCalc", false);
		}

		private void SetBizCalcOnly(bool val)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences (this);
			ISharedPreferencesEditor editor = prefs.Edit ();
			editor.PutBoolean ("BizCalcOnly", val);
			editor.Apply ();
		}

		private void SetHideBizCalc(bool val)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences (this);
			ISharedPreferencesEditor editor = prefs.Edit ();
			editor.PutBoolean ("HidesBizCalc", val);
			editor.Apply ();
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
		
			switch (item.ItemId) {
			case Resource.Id.calendar:
				this.StartActivity (new Intent (this, typeof(CalActivity)));
				break;

			case Resource.Id.schedule:
				this.StartActivity (new Intent(Intent.ActionView, Android.Net.Uri.Parse(Api.SCHEDULE_URL)));
				break;

			case Resource.Id.callout:
				this.StartActivity (new Intent(Intent.ActionView, Android.Net.Uri.Parse(Api.CALLOUT_URL)));
				break;

			case Resource.Id.bchide:
				this.SetHideBizCalc (true);
				this.SetBizCalcOnly (false);
				break;

			case Resource.Id.bconly:
				this.SetHideBizCalc (false);
				this.SetBizCalcOnly (true);
				break;

			case Resource.Id.showall:
				this.SetHideBizCalc (false);
				this.SetBizCalcOnly (false);
				break;
			}
				
			return base.OnOptionsItemSelected (item);
		}

		async private void UpdateData()
		{
			var req = await Api.GetRequests();
			if (this.GetBizCalcOnly ()) {
				req.Requests = req.Requests.Where (x => x.IsCalc ()).ToList ();
			}

			if (this.GetHideBizCalc ()) {
				req.Requests = req.Requests.Where (x => x.IsCalc () == false).ToList ();
			}

			RunOnUiThread (() => {
				if (Api.Updated == new DateTime(0)) {
					this.ActionBar.Title = "Update Failed";
				} else {
					this.ActionBar.Title = Api.Updated.ToString("G");
				}
				this.ListAdapter = new RequestAdapter (this, req);
			});
		}
	}
}


