## ![](Common/circle_image_icon.png)Image Carousel Control Plugin for Xamarin.Forms

Simple but elegant way of display circle images in your Xamarin.Forms projects

#### Setup
* Available on NuGet: https://www.nuget.org/packages/Xamd.Plugins.Forms.ImageCarousel
* Install into your PCL project and Client projects.

In your iOS, Android, and Windows Phone projects call:

```
Xamarin.Forms.Init();//platform specific init
ImageCarouselRenderer.Init();
```

You must do this AFTER you call Xamarin.Forms.Init();

#### Usage
Create a collection of FileImageSources.

In your ImageCarousel constructor pass in the the collection.
```
var images = new [] {
  new FileImageSource { File = "mickey.png" },
  new FileImageSource { File = "goofy.gif" },
  new FileImageSource { File = "pluto.jpg" }
};
new ImageCarousel (images) {
  HorizontalOptions = LayoutOptions.FillAndExpand,
  VerticalOptions = LayoutOptions.FillAndExpand
}
```

**XAML:**

First add the xmlns namespace:
```xml
xmlns:controls="clr-namespace:Xamd.ImageCarousel.Forms.Plugin.Abstractions;assembly=Xamd.ImageCarousel.Forms.Plugin.Abstractions"
```

Then add the xaml:

```xml
<controls:ImageCarousel Images="{Binding Images}" HorizontalOptions="Center" />
```


**Bindable Properties**

You are able to set the ```Images``` as a bindable collection.


#### Contributors
* [Andres Castro](https://github.com/acastr7)

Thanks!

#### License
Licensed under main repo license
