using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Text;
using Android.Text.Method;
using TeaLoverApp.Models;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Firebase;
using Firebase.Auth;
using static Android.Views.View;
using Android.Gms.Tasks;
using Android.Support.Design.Widget;
using Java.Lang;
using Xamarin.Facebook;
using Xamarin.Facebook.Login.Widget;

namespace TeaLoverApp.Activities
{
    public class PageFragment : Fragment, IOnCompleteListener
    {
        const string ARG_PAGE = "ARG_PAGE";
        private int mPage;

        private View view;
        private string checkTask;

        private Button button_register;
        private Button button_login;
        public LoginButton button_facebook;
        private static TextView register_text_name;
        private static TextView register_text_email;
        private TextView register_text_password;
        private TextView login_text_email;
        private TextView login_text_password;
        private TextView register_link_agree;
        private TextView login_link_forgot;

        private const string FirebaseURL = "https://tealover-ad9e4.firebaseio.com/";

        public static FirebaseApp app;
        FirebaseAuth auth;

        private ICallbackManager mFBCallManager;
        private MyProfileTracker mprofileTracker;

        public static PageFragment newInstance(int page)
        {
            var args = new Bundle();
            args.PutInt(ARG_PAGE, page);
            var fragment = new PageFragment();
            fragment.Arguments = args;
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mPage = Arguments.GetInt(ARG_PAGE);
            // Init firebase auth
            InitFirebaseAuth();
        }

        private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder()
                .SetApplicationId("1:179824068904:android:4ab6e50e537bae9f")
                .SetApiKey("AIzaSyDAwOZA1xf142rAvRZcFIITRgs0GAEPllM")
                .Build();

            if(app == null)
            {
                app = FirebaseApp.InitializeApp(Context, options);
            }

            auth = FirebaseAuth.GetInstance(app);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // get inflater
            view = inflater.Inflate(Resource.Layout.fragment_page, container, false);

            // set view for fragment
            if (mPage == 1)
            {
                view = inflater.Inflate(Resource.Layout.Login, container, false);
            }
            else
            {
                view = inflater.Inflate(Resource.Layout.Register, container, false);
            }

            register_text_name = (TextView)view.FindViewById(Resource.Id.register_text_name);
            register_text_email = (TextView)view.FindViewById(Resource.Id.register_text_email);
            register_text_password = (TextView)view.FindViewById(Resource.Id.register_text_password);
            register_link_agree = (TextView)view.FindViewById(Resource.Id.register_link_agree);
            login_link_forgot = (TextView)view.FindViewById(Resource.Id.login_link_forgot);
            login_text_email = (TextView)view.FindViewById(Resource.Id.login_text_email);
            login_text_password = (TextView)view.FindViewById(Resource.Id.login_text_password);
            button_register = (Button)view.FindViewById(Resource.Id.register_button_register);
            button_login = (Button)view.FindViewById(Resource.Id.login_button_login);
            button_facebook = (LoginButton)view.FindViewById(Resource.Id.login_button_facebook);

            // add link to conditions of use 
            if (register_link_agree != null)
            {
                register_link_agree.TextFormatted = Html.FromHtml("<span>By creating an account, you agree to our <a href='condition://'>Conditions of Use</a>. </span>");
                register_link_agree.MovementMethod = LinkMovementMethod.Instance;
            }

            if (login_link_forgot != null)
            {
                // set click event
                login_link_forgot.Click += delegate
                {
                    Intent intent = new Intent(Context, typeof(ForgotPassword));
                    StartActivity(intent);
                };
            }

            if (button_register != null && register_text_email != null && register_text_password != null)
            {
                button_register.Click += delegate
                {
                    RegisterUser(register_text_email.Text, register_text_password.Text);
                };
            }

            if (button_login != null && login_text_email != null && login_text_password != null)
            {
                button_login.Click += delegate
                {
                    LoginUser(login_text_email.Text, login_text_password.Text);
                };
            }

            if (button_facebook != null)
            {
            }
            return view;
        }

        private void RegisterUser(string email, string password)
        {
            checkTask = "register";
            auth.CreateUserWithEmailAndPassword(email, password)
                .AddOnCompleteListener(this);
        }

        private void LoginUser(string email, string password)
        {
            checkTask = "login";
            auth.SignInWithEmailAndPassword(email, password)
                .AddOnCompleteListener(this);
        }

        public async void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                if (checkTask.Equals("register"))
                {
                    var user = auth.CurrentUser;
                    Account acc = new Account();
                    acc.fullname = register_text_name.Text;

                    var firebase = new FirebaseClient(FirebaseURL);

                    //Add item
                    await firebase.Child("users/" + user.Uid).PutAsync<Account>(acc);           
                }
                Intent intent = new Intent(this.Context, typeof(DashBoard));
                StartActivity(intent);            
            }
            else
            {
                if (checkTask.Equals("login"))
                {
                    Snackbar snackBar = Snackbar.Make(view, "Email or password is not correct.", Snackbar.LengthShort);
                    snackBar.Show();

                    View view1 = snackBar.View;
                    TextView txtv = (TextView)view1.FindViewById(Resource.Id.login_error_message);
                }
                else
                {
                    Snackbar snackBar = Snackbar.Make(view, "Register failed.", Snackbar.LengthShort);
                    snackBar.Show();

                    View view1 = snackBar.View;
                    TextView txtv = (TextView)view1.FindViewById(Resource.Id.register_error_message);
                }
            }
        }

        /*public async override void OnActivityResult(int requestCode, int resultCode, Android.Content.Intent data)
        {           
            base.OnActivityResult(requestCode, resultCode, data);
            mFBCallManager.OnActivityResult(requestCode, (int)resultCode, data);

            var user = auth.CurrentUser;
            Console.WriteLine("abc ");
            if (user != null) Console.WriteLine("abc 123" + user.Email);
        }

        void IFacebookCallback.OnCancel()
        {
            Console.WriteLine("cancel");
            //throw new NotImplementedException();
        }

        void IFacebookCallback.OnError(FacebookException error)
        {
            Console.WriteLine("error");
        }

        void IFacebookCallback.OnSuccess(Java.Lang.Object result)
        {
            Console.WriteLine("success");        
        }*/

        /*public async void Register()
        {
            Account user = new Account();
            user.name = textRegisterName.Text;
            user.password = textRegisterPassword.Text;
            string email = textRegisterEmail.Text;

            var firebase = new FirebaseClient(FirebaseURL);

            //Add item
            await firebase.Child("users/"+email.Replace('.',',')).PutAsync<Account>(user);
        }*/
    }
}