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
using Xamarin.Facebook.Login.Widget;
using Android.Support.Design.Widget;
using Firebase;
using Firebase.Auth;
using Xamarin.Facebook;
using Android.Gms.Tasks;
using Java.Lang;
using Android.Support.V7.App;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using TeaLoverApp.Models;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Facebook.Login;

namespace TeaLoverApp.Activities
{
    [Activity(Label = "LoginActivity", Theme = "@style/MyTheme")]
    public class LoginActivity : AppCompatActivity, IOnCompleteListener, IFacebookCallback
    {
        private Button button_login;
        private LoginButton button_facebook;
        private TextView button_register;
        private TextView login_text_email;
        private TextView login_text_password;
        private TextView login_link_forgot;
        private LinearLayout activity_login;

        private const string FirebaseURL = "https://tealover-ad9e4.firebaseio.com/";

        public static FirebaseApp app;
        private FirebaseAuth auth;
        private ICallbackManager mFBCallManager;
        private MyProfileTracker mprofileTracker;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            FacebookSdk.SdkInitialize(ApplicationContext);
            
            // Init firebase auth
            InitFirebaseAuth();

            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Login";
            SupportActionBar.SetDisplayShowTitleEnabled(true);

            login_link_forgot = (TextView)FindViewById(Resource.Id.login_link_forgot);
            login_text_email = (TextView)FindViewById(Resource.Id.login_text_email);
            login_text_password = (TextView)FindViewById(Resource.Id.login_text_password);
            button_login = (Button)FindViewById(Resource.Id.login_button_login);
            button_facebook = (LoginButton)FindViewById(Resource.Id.login_button_facebook);
            button_register = (TextView)FindViewById(Resource.Id.login_button_register);
            activity_login = (LinearLayout)FindViewById(Resource.Id.activity_login);
          
            button_facebook.SetReadPermissions(new List<string> {
                "email",
                "public_profile"
            });
            mFBCallManager = CallbackManagerFactory.Create();
            button_facebook.RegisterCallback(mFBCallManager, this);
 
            login_link_forgot.Click += delegate
            {
                Intent intent = new Intent(this, typeof(ForgotPassword));
                StartActivity(intent);
            };

            button_login.Click += delegate
            {
                LoginUser(login_text_email.Text, login_text_password.Text);
            };

            button_register.Click += delegate
            {
                Intent intent = new Intent(this, typeof(RegisterActivity));
                StartActivity(intent);
            };
        }

        private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder()
                .SetApplicationId("1:179824068904:android:4ab6e50e537bae9f")
                .SetApiKey("AIzaSyDAwOZA1xf142rAvRZcFIITRgs0GAEPllM")
                .Build();
            if (app == null)
            {
                app = FirebaseApp.InitializeApp(this, options);
            }
            auth = FirebaseAuth.GetInstance(app);
        }

        private void LoginUser(string email, string password)
        {
            auth.SignInWithEmailAndPassword(email, password)
                .AddOnCompleteListener(this);
        }

        public async void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                Intent intent = new Intent(this, typeof(DashBoard));
                StartActivity(intent);
            }
            else
            {
                Snackbar snackBar = Snackbar.Make(activity_login, "Email or password is not correct.", Snackbar.LengthShort);
                snackBar.Show();

                View view1 = snackBar.View;
                TextView txtv = (TextView)view1.FindViewById(Resource.Id.login_error_message);
            }
        }

        void IFacebookCallback.OnCancel()
        {
            throw new NotImplementedException();
        }

        void IFacebookCallback.OnError(FacebookException error)
        {
            throw new NotImplementedException();
        }

        void IFacebookCallback.OnSuccess(Java.Lang.Object result)
        {
            // set facebook token
            LoginResult loginResult = result as LoginResult;
            handleFacebookAccessToken(loginResult.AccessToken);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            mFBCallManager.OnActivityResult(requestCode, (int)resultCode, data);
        }

        private void handleFacebookAccessToken(AccessToken accessToken)
        {
            AuthCredential credential = FacebookAuthProvider.GetCredential(accessToken.Token);
            auth.SignInWithCredential(credential).AddOnCompleteListener(this,this);
        }
    }
}