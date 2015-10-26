using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace StepCounter.Activities
{
	public abstract class BaseActivity : AppCompatActivity
	{
		public Toolbar Toolbar { get; set; }

		protected int ActionBarIcon { set { Toolbar.SetNavigationIcon (value); } }

		protected abstract int LayoutResource { get; }

		protected override void OnCreate (Bundle bundle)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (bundle);
			SetContentView (LayoutResource);
			Toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			if (Toolbar != null) {
				SetSupportActionBar (Toolbar);
				SupportActionBar.SetDisplayHomeAsUpEnabled (true);
				SupportActionBar.SetHomeButtonEnabled (true);
			}
		}
	}
}
