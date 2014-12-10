using System;
using System.Diagnostics;
using System.Text;
using Android.App;
using Android.Content;
using Android.Util;
using ByteSmith.WindowsAzure.Messaging;
using Gcm.Client;
using Photolane.Droid.View;
using Constants = Photolane.Shared.Helper.Constants;

namespace Photolane.Droid.Helper.Push
{
    [Service]
    public class GcmService : GcmServiceBase
    {
        public GcmService() : base(Constants.SenderID)
        {
            Log.Debug(NotificationBroadcastReceiver.TAG, "GCM Service constructor");
        }

        public static string RegistrationId { get; private set; }
        private NotificationHub Hub { get; set; }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            Log.Debug(NotificationBroadcastReceiver.TAG, "On Recoverable error");
            return false;
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Debug(NotificationBroadcastReceiver.TAG, "GCM Message Received!");

            var msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (string key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key));

                string messageText = intent.Extras.GetString("msg");
                if (!string.IsNullOrEmpty(messageText))
                {
                    CreateNotification("New hub message!", messageText);
                    return;
                }
            }

            CreateNotification("Unknown message details", msg.ToString());
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Debug(NotificationBroadcastReceiver.TAG, "On Error : "+errorId);
        }

        protected override async void OnRegistered(Context context, string registrationId)
        {
            Log.Debug(NotificationBroadcastReceiver.TAG, "GCM Registered : " + registrationId);
            RegistrationId = registrationId;

            CreateNotification("GcmService-GCM Registered...", "The device has been Registered, Tap to View!");

            Hub = new NotificationHub(Constants.NotificationHubPath, Constants.ConnectionString);
            try
            {
                await Hub.UnregisterAllAsync(registrationId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }

            //var tags = new List<string> {"falcons"};
            try
            {
                await Hub.RegisterNativeAsync(registrationId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }
        }

        private void CreateNotification(string title, string desc)
        {
            //Create notification
            var notificationManager = GetSystemService(NotificationService) as NotificationManager;

            //Create an intent to show ui
            var uiIntent = new Intent(this, typeof (MainActivity));

            //Create the notification
            var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title)
            {
                Flags = NotificationFlags.AutoCancel
            };

            //Auto cancel will remove the notification once the user touches it

            //Set the notification info
            //we use the pending intent, passing our ui intent over which will get called
            //when the notification is tapped.
            notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));

            //Show the notification
            if (notificationManager != null) notificationManager.Notify(1, notification);
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Debug(NotificationBroadcastReceiver.TAG, "On unregister");
            if (Hub != null)
            {
                Hub.UnregisterNativeAsync();
            }
        }
    }
}