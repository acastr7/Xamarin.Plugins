using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Xamd.ImageCarousel.Forms.Plugin.Abstractions
{
	public class ImageCarousel : AbsoluteLayout
	{
		public static readonly BindableProperty ImageUrlsProperty = BindableProperty.Create<ImageCarousel, ObservableCollection<string>> (p => p.ImageUrls, default(ObservableCollection<string>));

		public ObservableCollection<string> ImageUrls {
			get { return (ObservableCollection<string>)GetValue (ImageUrlsProperty); }
			set { SetValue (ImageUrlsProperty, value); }
		}
			
		private int _currentIndex = -1;
		public Image CurrentImage { get; private set; } = new Image();
		public Image NextImage { get; private set; } = new Image();
		public Image PrevImage { get; private set; } = new Image();

		public ImageCarousel ()
		{
			ImageUrls = new ObservableCollection<string> ();
			this.Children.Add (CurrentImage);
			this.Children.Add (NextImage);
			this.Children.Add (PrevImage);
		}
			
		void ImageUrls_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			InitDisplayImages();
		}

	
		void InitDisplayImages()
		{
			if ((ImageUrls.Count > 0) )
			{
				LoadImageFromSource(CurrentImage, ImageUrls[0]); 
				CurrentImage.Layout (new Rectangle (0, 0, this.Width, this.Height));
				_currentIndex = 0;
				var nextNumber = _currentIndex == ImageUrls.Count - 1 ? 0 : _currentIndex + 1;
				var prevNumber = _currentIndex == 0 ? ImageUrls.Count - 1 : _currentIndex - 1;
				LoadImageFromSource(NextImage, ImageUrls[nextNumber]); 
				LoadImageFromSource(PrevImage, ImageUrls[prevNumber]); 
				//ForceLayout ();
			}
		}

		private void LoadImageFromSource(Image image, string url)
		{
			var imageSource = new UriImageSource () {
				Uri = new Uri (url)
			};
			image.Aspect = Aspect.AspectFill;
			image.Source = imageSource;
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			base.LayoutChildren (x, y, width, height);
			NextImage.Layout (new Rectangle (this.Width, 0, this.Width, this.Height));
			CurrentImage.Layout (new Rectangle (0, 0, this.Width, this.Height));
			PrevImage.Layout (new Rectangle (-this.Width, 0, this.Width, this.Height));
		}

		protected override void OnPropertyChanged (string propertyName = null)
		{
			base.OnPropertyChanged (propertyName);

			if (propertyName == ImageUrlsProperty.PropertyName && ImageUrls != null) {
				ImageUrls.CollectionChanged += ImageUrls_CollectionChanged;;
				InitDisplayImages ();
			}
		}


		protected override void OnChildAdded (Element child)
		{
			base.OnChildAdded (child);
		}

		public async void OnSwipeLeft ()
		{
			await Task.WhenAll (CurrentImage.LayoutTo (new Rectangle (-this.Width, 0, this.Width, this.Height), 1000),
								NextImage.LayoutTo (new Rectangle (0, 0, this.Width, this.Height), 1000));
			LoadNextImage ();
		}

		private void LoadNextImage()
		{
			_currentIndex++;
			if (_currentIndex == ImageUrls.Count)
				_currentIndex = 0;

			var nextNumber = _currentIndex + 1;
			if (nextNumber == ImageUrls.Count)
				nextNumber = 0;

			var temp = PrevImage;
			PrevImage = CurrentImage;
			CurrentImage = NextImage;
			NextImage = temp;

			LoadImageFromSource(NextImage, ImageUrls[nextNumber]);
			ForceLayout ();
		}

		public async void OnSwipeRight ()
		{
			await Task.WhenAll (CurrentImage.LayoutTo (new Rectangle (this.Width, 0, this.Width, this.Height), 1000),
				PrevImage.LayoutTo (new Rectangle (0, 0, this.Width, this.Height), 1000));
			LoadPrevImage ();
		}
			
		private void LoadPrevImage()
		{
			_currentIndex--;
			if (_currentIndex == -1)
				_currentIndex = ImageUrls.Count - 1;

			var prevIndex = _currentIndex - 1;
			if (prevIndex == -1)
				prevIndex = ImageUrls.Count - 1;

			var temp = NextImage;
			NextImage = CurrentImage;
			CurrentImage = PrevImage;
			PrevImage = temp;

			LoadImageFromSource(PrevImage, ImageUrls[prevIndex]);
			ForceLayout ();
		}
	}
}

