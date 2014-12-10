using System;
using System.Net;
using System.Reflection;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;

namespace Photolane.Droid.Helper
{
    public static class ListUtils
    {
        public static LruCache Cache { get; set; }

        /// <summary>
        ///     Get Worker Bitmap which associated with particular Image View
        ///     Function comes from
        ///     <a href="https://developer.android.com/training/displaying-bitmaps/process-bitmap.html">Android Developer</a>
        /// </summary>
        /// <param name="imageView">image view which bitmapworkertask associated</param>
        /// <returns>Bitmap Worker Task Which work in particular ImageView</returns>
        public static BitmapWorkerTask GetBitmapWorkerTask(ImageView imageView)
        {
            if (imageView == null) return null;
            Drawable drawable = imageView.Drawable;
            if (!(drawable is AsyncDrawable)) return null;
            var asyncDrawable = drawable as AsyncDrawable;
            return asyncDrawable.BitmapWorkerTask;
        }

        /// <summary>
        ///     Check if another running task is already associated with the Image View
        ///     Function comes from
        ///     <a href="https://developer.android.com/training/displaying-bitmaps/process-bitmap.html">Android Developer</a>
        /// </summary>
        /// <param name="url">url for image to download</param>
        /// <param name="imageView">image view to bind image</param>
        /// <returns></returns>
        private static bool CancelPotentialWork(string url, ImageView imageView)
        {
            BitmapWorkerTask bitmapWorkerTask = GetBitmapWorkerTask(imageView);
            if (bitmapWorkerTask == null) return true;
            string bitmapData = bitmapWorkerTask.Data;
            //if bitmapData is not yet set or it differs from the new data
            if (bitmapData == "" || bitmapData != url)
            {
                //Cancel previous task
                bitmapWorkerTask.Cancel(true);
            }
            else
            {
                //the same work is already in progress
                return false;
            }
            //no task associated with the image view or an existing task was cancelled
            return true;
        }

        /// <summary>
        ///     Load Bitmap In Background Thread
        ///     Function comes from
        ///     <a href="https://developer.android.com/training/displaying-bitmaps/process-bitmap.html">Android Developer</a>
        /// </summary>
        /// <param name="url">url where image hosted</param>
        /// <param name="imageView">image view where image binded</param>
        /// <param name="loadingImage">Image default when Image from url not yet downloaded</param>
        public static void LoadBitmap(string url, Bitmap loadingImage, ImageView imageView)
        {
            if (!CancelPotentialWork(url, imageView)) return;
            var task = new BitmapWorkerTask(imageView);
            var asyncDrawable = new AsyncDrawable(Application.Context.Resources, loadingImage, task);
            var image = Cache.Get(url) as Bitmap;
            if (image != null)
            {
                Log.Debug("ds", "image not null");
                imageView.SetImageBitmap(image);
            }
            else
            {
                imageView.SetImageDrawable(asyncDrawable);
                task.Execute(url);
            }
        }

        /// <summary>
        ///     Convert image from URL to bitmap
        /// </summary>
        /// <param name="url">url where the image hosted</param>
        /// <returns></returns>
        public static Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            try
            {
                using (var webClient = new WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodBase currentMethod = MethodBase.GetCurrentMethod();
                if (currentMethod.DeclaringType != null)
                    Console.WriteLine("CLASS : {0}; METHOD : {1}; EXCEPTION : {2}", currentMethod.DeclaringType.FullName,
                        currentMethod.Name, ex.Message);
            }

            return imageBitmap;
        }
    }
}