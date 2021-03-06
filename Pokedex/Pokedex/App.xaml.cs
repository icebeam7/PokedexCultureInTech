using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pokedex
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.PokemonListView())
            {
                BarBackgroundColor = Color.Red,
                BarTextColor = Color.White
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
