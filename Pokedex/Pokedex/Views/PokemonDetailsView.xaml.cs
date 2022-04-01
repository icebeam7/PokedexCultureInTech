using Pokedex.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pokedex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PokemonDetailsView : ContentPage
    {
        public PokemonDetailsView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var vm = (PokemonDetailsViewModel)this.BindingContext;
            var text = vm.SelectedPokemon.Species.Description;

            await Services.TextToSpeechService.Speak(text, "en");
        }
    }
}