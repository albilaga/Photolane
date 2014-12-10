using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Gcm.Client;
using Microsoft.WindowsAzure.MobileServices;
using Photolane.Shared.Model;
using Constants = Photolane.Shared.Helper.Constants;
using Uri = Android.Net.Uri;

namespace Photolane.Droid.Helper
{
    public class AppHelper
    {
        public AppHelper(Context context)
        {
            InitApp();
            RegisterWithGcm(context);
        }

        //azure client
        public static MobileServiceClient Client { get; private set; }

        //timeline items
        public static ObservableCollection<Photo> TimelineItems { get; private set; }

        public static MobileServiceUser CurrentUser { get; set; }

        private void InitApp()
        {
            CurrentPlatform.Init();
            Client = new MobileServiceClient(Constants.MobileServiceUrl, Constants.MobileServiceKey);
        }

        private void RegisterWithGcm(Context context)
        {
            // Check to ensure everything's setup right
            GcmClient.CheckDevice(context);
            GcmClient.CheckManifest(context);

            // Register for push notifications
            Debug.WriteLine("Registering...");
            GcmClient.Register(context, Constants.SenderID);
        }

        public static bool IsLogin()
        {
            if (CurrentUser == null)
            {
                if (IsoStorage.Load(Constants.StateUserId).Length != 0)
                {
                    string userId = IsoStorage.Load(Constants.StateUserId);
                    string userToken = IsoStorage.Load(Constants.StateUserToken);

                    CurrentUser = new MobileServiceUser(userId) {MobileServiceAuthenticationToken = userToken};
                    return true;
                }
                return false;
            }
            return true;
        }


        public static async Task<MobileServiceUser> Authenticate(Context context)
        {
            try
            {
                MobileServiceUser user =
                    await Client.LoginAsync(context, MobileServiceAuthenticationProvider.Facebook);
                CurrentUser = user;
                //save to local
                IsoStorage.Save(Constants.StateUserId, user.UserId);
                IsoStorage.Save(Constants.StateUserToken, user.MobileServiceAuthenticationToken);
                return user;
            }
            catch (Exception ex)
            {
                Log.Debug("photolane", ex.Message);
                return null;
            }
        }

        /// <summary>
        ///     Decode Uri to bitmap and resize it to required size
        /// </summary>
        /// <param name="c">Activity</param>
        /// <param name="uri">uri from the image</param>
        /// <param name="requiredSize">size to be resized</param>
        /// <returns>bitmap</returns>
        public static Bitmap DecodeUri(Context c, Uri uri, int requiredSize)
        {
            try
            {
                var o = new BitmapFactory.Options {InJustDecodeBounds = true};
                BitmapFactory.DecodeStream(c.ContentResolver.OpenInputStream(uri), null, o);

                int widthTmp = o.OutWidth
                    ,
                    heightTmp = o.OutHeight;
                int scale = 1;

                while (true)
                {
                    if (widthTmp/2 < requiredSize || heightTmp/2 < requiredSize)
                        break;
                    widthTmp /= 2;
                    heightTmp /= 2;
                    scale *= 2;
                }

                var o2 = new BitmapFactory.Options {InSampleSize = scale};
                return BitmapFactory.DecodeStream(c.ContentResolver.OpenInputStream(uri), null, o2);
            }
            catch (Exception ex)
            {
                Log.Debug("photolane", ex.Message);
                return null;
            }
        }

        public static async Task LoadTimelineItems()
        {
            try
            {
                IMobileServiceTable<Photo> photoTable = Client.GetTable<Photo>();
                IEnumerable<Photo> result = await photoTable.ToEnumerableAsync();


                TimelineItems = new ObservableCollection<Photo>(result.OrderByDescending(item => item.PhotoTimeStamp));
            }
            catch (Exception ex)
            {
                Log.Debug("photolane", ex.Message);
            }
        }
    }
}