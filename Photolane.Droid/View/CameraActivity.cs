using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Util;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Photolane.Droid.Helper;
using Photolane.Shared.Model;
using Uri = Android.Net.Uri;

namespace Photolane.Droid.View
{
    [Activity(Label = "InsertFile")]
    public class CameraActivity : Activity
    {
        private ImageView _AddFileImageButton;
        private byte[] _BitmapData;
        private EditText _CaptionEditText;
        private string _FilePath;
        private Button _SendButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.CameraLayout);

            _AddFileImageButton = FindViewById<ImageView>(Resource.Id.addFileImageButton);
            _CaptionEditText = FindViewById<EditText>(Resource.Id.captionEditText);
            _SendButton = FindViewById<Button>(Resource.Id.sendButton);
            _SendButton.Click += OnClickedSendButton;
            _AddFileImageButton.Click += OnClickedAddFileImageButton;
        }

        private async void OnClickedSendButton(object sender, EventArgs e)
        {
            var photo = new Photo {Caption = _CaptionEditText.Text, File = _FilePath};

            //versi login
            AppHelper.Client.CurrentUser = AppHelper.CurrentUser;
            photo.UserId = AppHelper.Client.CurrentUser.UserId;

            //versi direct
            await AddPhoto(photo);

            bool result = await UploadPhoto(photo);
            if (result)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Finish();
            }
            else
            {
                var alertBuilder = new AlertDialog.Builder(this);
                alertBuilder.SetTitle("Error");
                alertBuilder.SetMessage("error when upload data");
                alertBuilder.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                alertBuilder.SetNeutralButton("ok", (senderArgs, args) => alertBuilder.Dispose());
                RunOnUiThread(() => alertBuilder.Show());
            }
        }

        /// <summary>
        ///     Add Photo to local
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        private async Task AddPhoto(Photo photo)
        {
            IMobileServiceTable<Photo> photoTable = AppHelper.Client.GetTable<Photo>();
            await photoTable.InsertAsync(photo);
        }

        /// <summary>
        ///     Upload photo to Azure Storage
        /// </summary>
        private async Task<bool> UploadPhoto(Photo photo)
        {
            try
            {
                // If we have a returned SAS, then upload the blob.
                if (!string.IsNullOrEmpty(photo.SasQueryString))
                {
                    // Get the URI generated that contains the SAS 
                    // and extract the storage credentials.
                    var cred = new StorageCredentials(photo.SasQueryString);
                    var imageUri = new System.Uri(photo.File);

                    // Instantiate a Blob store container based on the info in the returned item.
                    var container = new CloudBlobContainer(
                        new System.Uri(string.Format("https://{0}/{1}",
                            imageUri.Host, photo.ContainerName)), cred);

                    // Upload the new image as a BLOB from the stream.
                    CloudBlockBlob blobFromSasCredential =
                        container.GetBlockBlobReference(photo.ResourceName);
                    await blobFromSasCredential.UploadFromByteArrayAsync(_BitmapData, 0, _BitmapData.Length);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Debug("photolane", ex.Message);
                return false;
            }
        }

        private void OnClickedAddFileImageButton(object sender, EventArgs e)
        {
            //_AddFileImageButton.Click -= OnClickedAddFileImageButton;
            var intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select picture"), 1000);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 1000 && resultCode == Result.Ok && data != null)
            {
                Uri uri = data.Data;
                _FilePath = GetPathToImage(uri);
                Bitmap bmp = AppHelper.DecodeUri(this, uri, 300);
                _AddFileImageButton.SetImageBitmap(bmp);
                using (var stream = new MemoryStream())
                {
                    bmp.Compress(Bitmap.CompressFormat.Jpeg, 30, stream);
                    _BitmapData = stream.ToArray();
                }
            }
        }

        public override void OnBackPressed()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Finish();
            base.OnBackPressed();
        }

        private string GetPathToImage(Uri uri)
        {
            string path = null;
            // The projection contains the columns we want to return in our query.
            string[] projection = {MediaStore.Images.Media.InterfaceConsts.Data};
            using (ICursor cursor = ManagedQuery(uri, projection, null, null, null))
            {
                if (cursor != null)
                {
                    int columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    path = cursor.GetString(columnIndex);
                }
            }
            return path;
        }
    }
}