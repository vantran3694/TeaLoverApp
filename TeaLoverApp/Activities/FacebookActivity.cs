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
using Java.Lang;
using Xamarin.Facebook;
using Xamarin.Facebook.Login.Widget;

namespace TeaLoverApp.Activities
{
    [Activity(Label = "FacebookActivity")]
    public class FacebookActivity : Activity, IFacebookCallback
    {
        private ICallbackManager mFBCallManager;
        private MyProfileTracker mprofileTracker;
        LoginButton BtnFBLogin;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            FacebookSdk.SdkInitialize(ApplicationContext);
            mprofileTracker = new MyProfileTracker();
            //mprofileTracker.mOnProfileChanged += mProfileTracker_mOnProfileChanged;
            mprofileTracker.StartTracking();
            // Set our view from the "main" layout resource  
            //SetContentView(Resource.Layout.facebook);
            /*BtnFBLogin = FindViewById<LoginButton>(Resource.Id.fblogin);
            TxtFirstName = FindViewById<TextView>(Resource.Id.TxtFirstname);
            TxtLastName = FindViewById<TextView>(Resource.Id.TxtLastName);
            TxtName = FindViewById<TextView>(Resource.Id.TxtName);
            mprofile = FindViewById<ProfilePictureView>(Resource.Id.ImgPro);*/
            BtnFBLogin.SetReadPermissions(new List<string> {
                "user_friends",
                "public_profile"
            });
            mFBCallManager = CallbackManagerFactory.Create();
            BtnFBLogin.RegisterCallback(mFBCallManager, this);
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
            try
            {
                throw new NotImplementedException();
            }
            catch(System.Exception ex)
            {
                Console.WriteLine("abc " + ex.ToString());
            }
            
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            mFBCallManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}