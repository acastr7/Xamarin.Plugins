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

		public static readonly BindableProperty ImageUrlsProperty = BindableProperty.Create<ImageCarousel, ObservableCollection<string>> (p => p.ImageUrls, default(ObservableCollection<string>));

		public ObservableCollection<string> ImageUrls {
			get { return (ObservableCollection<string>)GetValue (ImageUrlsProperty); }
			set { SetValue (ImageUrlsProperty, value); }
		}
			

		public Image CurrentImage { get; private set; }

		public ImageCarousel ()
		{
			Images = new ObservableCollection<Image> ();
			ImageUrls = new ObservableCollection<string> ();
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

		void ImageUrls_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			foreach (var item in e.NewItems) {
				var url = item as string;
				if (url != null) {
					addEmptyImageAsChild (url);
				}
			}
		}

		void addImagesAsChildren ()
		{
			foreach (var image in Images) {
				addImageAsChild (image);
			}
		}

		void addEmptyImagesAsChildren ()
		{
			foreach (string url in ImageUrls) {
				addEmptyImageAsChild (url);
			}

		}

		void addImageAsChild (Image image)
		{
			var point = Point.Zero;
			this.Children.Add (image, new Rectangle (point, new Size (1, 1)), AbsoluteLayoutFlags.SizeProportional);
			//point = new Point (point.X + image.Width, 0);
		}

		void addEmptyImageAsChild (string url)
		{
			var point = Point.Zero;
			Image image = new Image ();
			//image.Source = new UriImageSource (){ Uri = new Uri( url) };
			image.HeightRequest = this.Height;
			image.WidthRequest = this.Width;
			Images.Add (image);
		//	addImageAsChild (image);
			//this.Children.Add (image, new Rectangle (point, new Size (1, 1)), AbsoluteLayoutFlags.SizeProportional);
			//point = new Point (point.X + image.Width, 0);
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
			if (propertyName == ImageUrlsProperty.PropertyName && ImageUrls != null) {
				ImageUrls.CollectionChanged += ImageUrls_CollectionChanged;;
				addEmptyImagesAsChildren ();
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
					LoadImageFromSource (CurrentImage);
				}
			}
		}

		private void LoadImageFromSource(Image image)
		{
			if (image.Source != null)
				return;
			
			var imageNumber = Images.IndexOf (image);
			var imageUri = ImageUrls [imageNumber];
			var imageSource = new UriImageSource () {
				Uri = new Uri (imageUri)
			};
			image.Source = imageSource;
		}

		public void OnSwipeLeft ()
		{
			var imageNumber = Images.IndexOf (CurrentImage);
			var nextNumber = imageNumber == Images.Count - 1 ? 0 : imageNumber + 1;
			var nextImage = Images [nextNumber];

			LoadImageFromSource (nextImage);
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

		    LoadImageFromSource (nextImage);

			//make sure this image is in position to be animated in
			nextImage.Layout (new Rectangle (new Point (-this.Width, 0), this.Bounds.Size));

			var current = CurrentImage;

			current.LayoutTo (new Rectangle ((this.Width), 0, this.Width, this.Height),2000);
			CurrentImage = nextImage;
			nextImage.LayoutTo (new Rectangle (0, 0, this.Width, this.Height),2000);
		}
			

	}
}

