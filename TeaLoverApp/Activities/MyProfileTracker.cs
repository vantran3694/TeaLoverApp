using System;
using Xamarin.Facebook;

namespace TeaLoverApp.Activities
{
    public class MyProfileTracker : ProfileTracker
    {
        public event EventHandler<OnProfileChangedEventArgs> mOnProfileChanged;
        protected override void OnCurrentProfileChanged(Profile oldProfile, Profile newProfile)
        {
            if (mOnProfileChanged != null)
            {
                mOnProfileChanged.Invoke(this, new OnProfileChangedEventArgs(newProfile));
            }
        }
    }
    public class OnProfileChangedEventArgs : EventArgs
    {
        public Profile mProfile;
        public OnProfileChangedEventArgs(Profile profile)
        {
            mProfile = profile;
        }
    }
}