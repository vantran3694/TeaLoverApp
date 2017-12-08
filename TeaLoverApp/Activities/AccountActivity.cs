using System;
using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Widget;
using Android.Text;
using Android.Content;
using Android.Views;

namespace TeaLoverApp.Activities
{
    [Activity(Label = "Login_RegisterActivity", MainLauncher = true, Theme = "@style/MyTheme")]
    public class AccountActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Account);

            // Find views
            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            var adapter = new CustomPagerAdapter(this, SupportFragmentManager);
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.my_toolbar);

            // Setup Toolbar
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Login/Register";

            // Set adapter to view pager
            pager.Adapter = adapter;

            // Setup tablayout with view pager
            tabLayout.SetupWithViewPager(pager);

            // get infalter view
            /*LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View v = inflater.Inflate(Resource.Layout.Login, null);
            Button btnForget = (Button)v.FindViewById(Resource.Id.link_forPass);*/
        }
    }
}