using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pokedex
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //await Services.PokemonService.GetPokemonListPerPage(0, 10);

            //await Services.TextToSpeechService.Speak("ほかの　木々を　自在に　操る。 森を　荒らす　人間は　死ぬまで 森から　出られないようにするのだ。", "ja");
            //await Services.TextToSpeechService.Speak("Puede controlar los árboles a su voluntad. Retiene hasta el fin de sus días a los humanos que se adentran en el bosque y lo mancillan.", "es");
            //await Services.TextToSpeechService.Speak("Small roots that extend from the tips of this Pokémon’s feet can tie into the trees of the forest and give Trevenant control over them.", "en");
        }
    }
}
