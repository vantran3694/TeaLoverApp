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
using Android.Gms.Tasks;
using Firebase;
using Firebase.Auth;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Text;
using Android.Text.Method;
using TeaLoverApp.Models;
using Firebase.Xamarin.Database;
using Android.Support.Design.Widget;

namespace TeaLoverApp.Activities
{
    [Activity(Label = "RegisterActivity", Theme = "@style/MyTheme")]
    public class RegisterActivity : AppCompatActivity, IOnCompleteListener
    {
        private Button button_register;
        private static TextView register_text_name;
        private TextView register_text_email;
        private TextView register_text_password;
        private TextView register_link_agree;
        private LinearLayout activity_register;

        private const string FirebaseURL = "https://tealover-ad9e4.firebaseio.com/";

        //public static FirebaseApp app;
        FirebaseAuth auth;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);

            // Init firebase auth
            InitFirebaseAuth();

            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Register";
            SupportActionBar.SetDisplayShowTitleEnabled(true);

            register_text_name = (TextView)FindViewById(Resource.Id.register_text_name);
            register_text_email = (TextView)FindViewById(Resource.Id.register_text_email);
            register_text_password = (TextView)FindViewById(Resource.Id.register_text_password);
            register_link_agree = (TextView)FindViewById(Resource.Id.register_link_agree);
            button_register = (Button)FindViewById(Resource.Id.register_button_register);
            activity_register = (LinearLayout)FindViewById(Resource.Id.activity_register);

            register_link_agree.TextFormatted = Html.FromHtml("<span>By creating an account, you agree to our <a href='condition://'>Conditions of Use</a>. </span>");
            register_link_agree.MovementMethod = LinkMovementMethod.Instance;

            button_register.Click += delegate
            {
                RegisterUser(register_text_email.Text, register_text_password.Text);
            };
        }

        private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder()
                .SetApplicationId("1:179824068904:android:4ab6e50e537bae9f")
                .SetApiKey("AIzaSyDAwOZA1xf142rAvRZcFIITRgs0GAEPllM")
                .Build();
            if (LoginActivity.app == null)
            {
                LoginActivity.app = FirebaseApp.InitializeApp(this, options);
            }
            auth = FirebaseAuth.GetInstance(LoginActivity.app);
        }

        private void RegisterUser(string email, string password)
        {
            auth.CreateUserWithEmailAndPassword(email, password)
                .AddOnCompleteListener(this);
        }

        public async void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                var user = auth.CurrentUser;
                /*user.SendEmailVerification();
                if (user.IsEmailVerified)
                {*/
                    Account acc = new Account();
                    acc.id = user.Uid;
                    acc.fullname = register_text_name.Text;
                    acc.email = user.Email;

                    var firebase = new FirebaseClient(FirebaseURL);

                    //Add item
                    await firebase.Child("users/" + user.Uid).PutAsync<Account>(acc);

                    Intent intent = new Intent(this, typeof(DashBoard));
                    StartActivity(intent);
                /*}
                else
                {*/
                //}
            }               
            else
            {
                Snackbar snackBar = Snackbar.Make(activity_register, "Register failed.", Snackbar.LengthShort);
                snackBar.Show();

                View view1 = snackBar.View;
                TextView txtv = (TextView)view1.FindViewById(Resource.Id.register_error_message);
            }
        }
    }
}