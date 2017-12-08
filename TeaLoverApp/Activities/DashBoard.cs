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
using Android.Support.V7.App;
using Firebase.Auth;
using static Android.Views.View;
using Android.Gms.Tasks;
using Android.Support.Design.Widget;
using Java.Lang;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using TeaLoverApp.Models;

namespace TeaLoverApp.Activities
{
    [Activity(Label = "DashBoard", Theme = "@style/MyTheme")]
    public class DashBoard : AppCompatActivity, IOnClickListener, IOnCompleteListener
    {
        private TextView dashboard_text_welcome;
        private EditText dashboard_new_password;
        private Button button_change_pass;
        private Button button_logout;

        private LinearLayout activity_dashboard;
        private FirebaseAuth auth;
        private const string FirebaseURL = "https://tealover-ad9e4.firebaseio.com/";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DashBoard);

            if (LoginActivity.app != null)
            {
                auth = FirebaseAuth.GetInstance(LoginActivity.app);
            }

            RecordFacebookUser();

            dashboard_text_welcome = (TextView)FindViewById(Resource.Id.dashboard_welcome);
            dashboard_new_password = (EditText)FindViewById(Resource.Id.dasboard_new_password);
            button_change_pass = (Button)FindViewById(Resource.Id.dashboard_button_change_password);
            button_logout = (Button)FindViewById(Resource.Id.dashboard_button_logout);
            activity_dashboard = (LinearLayout)FindViewById(Resource.Id.activity_dashboard);
            button_change_pass.SetOnClickListener(this);
            button_logout.SetOnClickListener(this);

            dashboard_text_welcome.Text = "Welcome";
        }

        public void OnClick(View v)
        {
            if(v.Id == Resource.Id.dashboard_button_change_password)
            {
                ChangePassword(dashboard_new_password.Text);
            }
            else if(v.Id == Resource.Id.dashboard_button_logout)
            {
                LogoutUser();
            }
        }

        private async void RecordFacebookUser()
        {
            var firebase = new FirebaseClient(FirebaseURL);
            FirebaseUser currentUser = auth.CurrentUser;
            var users = await firebase
                .Child("users")
                .OrderByKey()
                .StartAt(currentUser.Uid)
                .EndAt(currentUser.Uid)
                .LimitToFirst(1)
                .OnceAsync<Account>();

            if (users.Count == 0)
            {
                Account acc = new Account();
                acc.id = currentUser.Uid;
                acc.fullname = currentUser.DisplayName;
                acc.email = currentUser.Email;
                await firebase.Child("users/" + currentUser.Uid).PutAsync<Account>(acc);
            }
        }

        private void LogoutUser()
        {
            auth.SignOut();
            if (auth.CurrentUser == null)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
        }

        private void ChangePassword(string input_new_password)
        {
            FirebaseUser user = auth.CurrentUser;
            user.UpdatePassword(input_new_password)
                .AddOnCompleteListener(this);
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                Snackbar snackBar = Snackbar.Make(activity_dashboard, "Password has been changed", Snackbar.LengthShort);
                snackBar.Show();
            }
        }
    }
}