using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Photolane.Droid.Helper;
using Photolane.Droid.View.Adapter;

namespace Photolane.Droid.View
{
    [Activity(Label = "Photolane",Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private AppHelper _AppHelper;
        private ImageButton _CameraImageButton;
        private ListView _TimelineListView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //_AppHelper = new AppHelper(this);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //find listview
            _TimelineListView = FindViewById<ListView>(Resource.Id.timelineListView);

            GetTimelineItems();

            _CameraImageButton = FindViewById<ImageButton>(Resource.Id.cameraImageButton);
            _CameraImageButton.Click += OnClickedCameraImageButton;
        }

        private async void GetTimelineItems()
        {
            await AppHelper.LoadTimelineItems();
            if (AppHelper.TimelineItems != null)
            {
                RunOnUiThread(() =>
                {
                    FindViewById<ProgressBar>(Resource.Id.progressBar).Visibility = ViewStates.Gone;
                    _TimelineListView.Adapter = new TimelineAdapter(this, AppHelper.TimelineItems);
                });
            }
        }

        protected override void OnResume()
        {
            GetTimelineItems();
            base.OnResume();
        }

        private void OnClickedCameraImageButton(object sender, EventArgs e)
        {
            StartActivity(typeof (CameraActivity));
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}