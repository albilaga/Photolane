using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using Photolane.Droid.Helper;

namespace Photolane.Droid.View
{
    [Activity(Label = "Photolane", MainLauncher = true,Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        private AppHelper _AppHelper;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _AppHelper = new AppHelper(this);
            if (AppHelper.IsLogin())
            {
                MoveToMainActivity();
            }
            else
            {
                SetContentView(Resource.Layout.LoginLayout);
                var loginButton = FindViewById<Button>(Resource.Id.loginButton);
                loginButton.Click += OnLoginButtonClicked;
            }
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            MobileServiceUser user = await AppHelper.Authenticate(this);
            if (user != null)
            {
                AppHelper.CurrentUser = user;
                MoveToMainActivity();
            }
        }

        private void MoveToMainActivity()
        {
            StartActivity(typeof (MainActivity));
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Finish();
        }
    }
}