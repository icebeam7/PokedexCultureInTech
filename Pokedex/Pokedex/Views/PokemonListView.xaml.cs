using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Pokedex.ViewModels;

namespace Pokedex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PokemonListView : ContentPage
    {
        PokemonListViewModel vm;

        public PokemonListView()
        {
            InitializeComponent();

            vm = new PokemonListViewModel();
            BindingContext = vm;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await Task.Run(() => vm.LoadPokemonDataCommand.Execute(null));
            vm.SelectedPokemon = null;
        }
    }
}