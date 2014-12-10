using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Photolane.Droid.Helper
{
    /// <summary>
    ///     Helper class for handle concurency in ListView
    ///     Class comes from
    ///     <a href="https://developer.android.com/training/displaying-bitmaps/process-bitmap.html">Android Developer</a>
    /// </summary>
    public class AsyncDrawable : BitmapDrawable
    {
        private readonly WeakReference<BitmapWorkerTask> _BitmapWorkerTaskReference;

        public AsyncDrawable(Resources res, Bitmap bitmap, BitmapWorkerTask bitmapWorkerTask)
            : base(res, bitmap)
        {
            _BitmapWorkerTaskReference = new WeakReference<BitmapWorkerTask>(bitmapWorkerTask);
        }

        public BitmapWorkerTask BitmapWorkerTask
        {
            get
            {
                BitmapWorkerTask bitmapWorkerTask;
                return _BitmapWorkerTaskReference.TryGetTarget(out bitmapWorkerTask) ? bitmapWorkerTask : null;
            }
        }
    }
}