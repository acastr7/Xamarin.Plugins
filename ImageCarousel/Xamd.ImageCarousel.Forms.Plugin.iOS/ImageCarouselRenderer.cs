using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using Xamd.ImageCarousel.Forms.Plugin.iOS;

[assembly: ExportRenderer (typeof(Xamd.ImageCarousel.Forms.Plugin.Abstractions.ImageCarousel), typeof(ImageCarouselRenderer))]
namespace Xamd.ImageCarousel.Forms.Plugin.iOS
{
	public class ImageCarouselRenderer : VisualElementRenderer<Xamd.ImageCarousel.Forms.Plugin.Abstractions.ImageCarousel>
	{
		UISwipeGestureRecognizer swipeLeftGestureRecognizer;
		UISwipeGestureRecognizer swipeRightGestureRecognizer;

		/// <summary>
		/// Used for registration with dependency service
		/// </summary>
		public static void Init ()
		{
			var temp = DateTime.Now;
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Xamd.ImageCarousel.Forms.Plugin.Abstractions.ImageCarousel> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				if (swipeLeftGestureRecognizer != null) {
					this.RemoveGestureRecognizer (swipeLeftGestureRecognizer);
				}

				if (swipeRightGestureRecognizer != null) {
					this.RemoveGestureRecognizer (swipeRightGestureRecognizer);
				}
			}

			if (e.NewElement != null) {
				swipeLeftGestureRecognizer = new UISwipeGestureRecognizer (Element.OnSwipeLeft);
				swipeLeftGestureRecognizer.Direction = UISwipeGestureRecognizerDirection.Left;
				swipeRightGestureRecognizer = new UISwipeGestureRecognizer (Element.OnSwipeRight);
				swipeRightGestureRecognizer.Direction = UISwipeGestureRecognizerDirection.Right;

				this.AddGestureRecognizer (swipeLeftGestureRecognizer);
				this.AddGestureRecognizer (swipeRightGestureRecognizer);
			}
		}
	}
}

