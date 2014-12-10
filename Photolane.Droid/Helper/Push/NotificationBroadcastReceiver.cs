using Android.App;
using Android.Content;
using Gcm.Client;
using Constants = Photolane.Shared.Helper.Constants;

namespace Photolane.Droid.Helper.Push
{
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new[] { "gemastik.photolane.droid" })]
    [IntentFilter(new[] {Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK},
        Categories = new[] { "gemastik.photolane.droid" })]
    [IntentFilter(new[] {Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY}, Categories = new[] {"gemastik.photolane.droid"})]
    public class NotificationBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public const string TAG = "NotificationBroadcaseReceiver-GCM";
        public static string[] SENDER_IDS = {Constants.SenderID};
    }
}