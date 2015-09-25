using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Xamd.ImageCarousel.Forms.Plugin.Abstractions
{
	public class ImageCarousel : AbsoluteLayout
	{
		//The Bindable Images property, if MVVM/binding context is desired
		public static readonly BindableProperty ImagesProperty = BindableProperty.Create<ImageCarousel, ObservableCollection<Image>> (p => p.Images, default(ObservableCollection<Image>));

		public ObservableCollection<Image> Images {
			get { return (ObservableCollection<Image>)GetValue (ImagesProperty); }
			set { SetValue (ImagesProperty, value); }
		}

		public Image CurrentImage { get; private set; }

		public ImageCarousel ()
		{
			Images = new ObservableCollection<Image> ();
		}

		void Images_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			foreach (var item in e.NewItems) {
				var image = item as Image;
				if (image != null) {
					addImageAsChild (image);
				}
			}
		}

		void addImagesAsChildren ()
		{
			foreach (var image in Images) {
				addImageAsChild (image);
			}
		}

		void addImageAsChild (Image image)
		{
			var point = Point.Zero;
			this.Children.Add (image, new Rectangle (point, new Size (1, 1)), AbsoluteLayoutFlags.SizeProportional);
			point = new Point (point.X + image.Width, 0);
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			base.LayoutChildren (x, y, width, height);

			//fix layout issues caused by base behavior, make sure these things are in the right place before swiping begins
			var point = Point.Zero;

			foreach (var image in Images) {
				image.Layout (new Rectangle (point, image.Bounds.Size));
				point = new Point (point.X + image.Width + this.Bounds.Width, 0);

			}
		}

		protected override void OnPropertyChanged (string propertyName = null)
		{
			base.OnPropertyChanged (propertyName);

			//if the Images property has changed, clear our ImageList of images and add all the new images as children
			if (propertyName == ImagesProperty.PropertyName && Images != null) {
				Images.CollectionChanged += Images_CollectionChanged;
				addImagesAsChildren ();
			}
		}

		protected override void OnChildAdded (Element child)
		{
			base.OnChildAdded (child);

			//each time a child Image is added, add it to the ImageList
			if (child is Image) {
				//set a CurrentImage if we don't already have one
				if (CurrentImage == null) {
					CurrentImage = (Image)child;
				}
			}
		}

		public void OnSwipeLeft ()
		{
			var imageNumber = Images.IndexOf (CurrentImage);
			var nextNumber = imageNumber == Images.Count - 1 ? 0 : imageNumber + 1;
			var nextImage = Images [nextNumber];

			//make sure this image is in position to be animated in
			nextImage.Layout (new Rectangle (new Point (CurrentImage.Width, 0), CurrentImage.Bounds.Size));

			var current = CurrentImage;

			current.LayoutTo (new Rectangle (-(this.Bounds.Width + this.Width + CurrentImage.Width), 0, CurrentImage.Width, CurrentImage.Height));
			CurrentImage = nextImage;
			nextImage.LayoutTo (new Rectangle (0, 0, CurrentImage.Width, CurrentImage.Height));
		}

		public void OnSwipeRight ()
		{
			var imageNumber = Images.IndexOf (CurrentImage);
			var nextNumber = imageNumber == 0 ? Images.Count - 1 : imageNumber - 1;
			var nextImage = Images [nextNumber];

			//make sure this image is in position to be animated in
			nextImage.Layout (new Rectangle (new Point (-CurrentImage.Width, 0), CurrentImage.Bounds.Size));

			var current = CurrentImage;

			current.LayoutTo (new Rectangle ((this.Bounds.Width + this.Width + CurrentImage.Width), 0, CurrentImage.Width, CurrentImage.Height));
			CurrentImage = nextImage;
			nextImage.LayoutTo (new Rectangle (0, 0, CurrentImage.Width, CurrentImage.Height));
		}
			

	}
}

