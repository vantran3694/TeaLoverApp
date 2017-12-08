using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

namespace TeaLoverApp.Activities
{
    [Activity(Label = "ConditionActivity", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    [IntentFilter(new[] { Intent.ActionView }, DataScheme = "condition", Categories = new[] { Intent.CategoryDefault })]
    public class ConditionActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ConditionOfUse);

            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbarCondition);

            // Setup Toolbar
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Conditions of Use";
            // Create your application here
        }
    }
}