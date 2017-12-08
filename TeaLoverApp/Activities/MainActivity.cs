using Android.App;
using Android.Widget;
using Android.OS;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Content;
using TeaLoverApp.Activities;

namespace TeaLoverApp
{
    [Activity(Label = "TeaLoverApp", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity
    {
        DrawerLayout drawerLayout;
        NavigationView navigationView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //change back arrow with icon
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.icon);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            //set action bar
            SupportActionBar.Title = "Tea Lover";
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            //call click event for left menu
            navigationView.NavigationItemSelected += HomeNavigationView_NavigationItemSelected;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void HomeNavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            var menuItem = e.MenuItem;
            menuItem.SetChecked(!menuItem.IsChecked);
            Intent intent;
            switch (menuItem.ItemId)
            {
                case Resource.Id.nav_account:
                    intent = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent);
                    break;
            }
        }
    }
}

