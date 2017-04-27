using System;

using Xamarin.Forms;

namespace UnitConverter
{
    public class App : Application
    {
        public App()
        {
            MainPage = new NavigationPage(new ConversionPage())
            {
                BarBackgroundColor = Color.FromHex("#548BB4"),
                BackgroundColor = Color.FromHex("#BECCD9"),
                BarTextColor = Color.White
            };
        }
    }
}

