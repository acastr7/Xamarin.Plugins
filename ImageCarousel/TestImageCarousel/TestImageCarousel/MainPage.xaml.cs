using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TestImageCarousel
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
			//btn.Clicked += Btn_Clicked;
			MyCarousel.Images = new System.Collections.ObjectModel.ObservableCollection<Image> ();
			ImageUrls = new List<string> () {
				"http://cdn.zenfolio.net/img/s10/v114/p93242074-3.jpg?sn=2YH",
				"http://cdn.zenfolio.net/img/s6/v137/p256857143-3.jpg?sn=2YH",
				"http://cdn.zenfolio.net/img/s5/v129/p116696782-3.jpg?sn=2YH",
				"http://cdn.zenfolio.net/img/s6/v150/p921067385-3.jpg?sn=2YH",
				"http://cdn.zenfolio.net/img/s7/v164/p1026119629-3.jpg?sn=2YH",
				"http://cdn.zenfolio.net/img/s6/v141/p17900921-3.jpg?sn=2YH",
				"http://cdn.zenfolio.net/img/s7/v155/p330729244-3.jpg?sn=2YH",
				"http://cdn.zenfolio.net/img/s6/v139/p61290126-3.jpg?sn=2YH"
			};
			Images = new List<Image> ();

			foreach (var url in ImageUrls) {
				var image = new Image ();
				image.Source = ImageSource.FromUri (new Uri (url));
				Images.Add (image);
			}
		}

		public void LoadImagesClicked (object sender, EventArgs e)
		{
//			foreach (var image in Images) {
//				MyCarousel.Images.Add (image);
//
//			}
			//MyCarousel.ImageUrls.Clear();
//			//MyCarousel.Images.Clear ();
			foreach (var url in ImageUrls) {
				MyCarousel.ImageUrls.Add (url);

			}
		}

		private static List<string> ImageUrls;
		private static List<Image> Images;
	}
}

