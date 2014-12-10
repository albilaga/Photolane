using System.Collections.ObjectModel;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Photolane.Droid.Helper;
using Photolane.Shared.Model;

namespace Photolane.Droid.View.Adapter
{
    public class TimelineAdapter : BaseAdapter<Photo>
    {
        private readonly Activity _Context;
        private readonly ObservableCollection<Photo> _PhotoItems;

        public TimelineAdapter(Activity context, ObservableCollection<Photo> photoItems)
        {
            _Context = context;
            _PhotoItems = photoItems;
        }

        public override int Count
        {
            get { return _PhotoItems.Count; }
        }

        public override Photo this[int position]
        {
            get { return _PhotoItems[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            Photo photo = _PhotoItems[position];
            Android.Views.View view = convertView ??
                                      _Context.LayoutInflater.Inflate(Resource.Layout.TimelineListView, parent, false);
            ListUtils.LoadBitmap(photo.File,
                BitmapFactory.DecodeResource(Application.Context.Resources, Android.Resource.Drawable.IcMenuGallery),
                view.FindViewById<ImageView>(Resource.Id.photoHolderImageView));
            ListUtils.LoadBitmap(photo.ProfilePicture,
                BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.gravatar),
                view.FindViewById<ImageView>(Resource.Id.profilePictureImageView));
            view.FindViewById<TextView>(Resource.Id.captionTextView).Text = photo.Caption;
            view.FindViewById<TextView>(Resource.Id.photoTimeStampTextView).Text = photo.RelativePhotoTimeStamp;
            return view;
        }
    }
}