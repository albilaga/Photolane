using Android.App;
using Android.Content;
using Gcm.Client;

namespace Photolane.Droid.Helper.Push
{
    [BroadcastReceiver]
    [IntentFilter(new[] {Intent.ActionBootCompleted})]
    internal class BootBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
    }
}