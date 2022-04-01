using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using Pokedex.Models;
using Pokedex.Services;
using Pokedex.Views;
using System.IO;

namespace Pokedex.ViewModels
{
    public class PokemonListViewModel : BaseViewModel
    {
        private List<MyPokemon> pokemon;

        public ObservableCollection<MyPokemon> PokemonCollection { get; set; }

        private MyPokemon selectedPokemon;

        public MyPokemon SelectedPokemon
        {
            get { return selectedPokemon; }
            set { SetProperty(ref selectedPokemon, value); }
        }

        public ICommand LoadPokemonDataCommand { get; private set; }
        public ICommand GoToPokemonDetailsCommand { get; private set; }
        public ICommand LoadMorePokemonDataCommand { get; private set; }
        public ICommand ScanCommand { get; private set; }

        private int pokemonPerPage = 20;
        private int currentPage = 0;

        private async Task LoadPokemonData()
        {
            if (PokemonCollection.Count == 0)
            {
                IsBusy = true;

                pokemon = await PokemonService.GetPokemonList(pokemonPerPage, currentPage);

                while (PokemonCollection.Count > 0)
                    PokemonCollection.RemoveAt(0);

                SetPokemonCollection(pokemon);

                IsBusy = false;
            }
        }

        private async Task LoadMorePokemonData()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            currentPage++;
            var morePokemon = await PokemonService.GetPokemonList(currentPage, pokemonPerPage);

            if (morePokemon?.Count > 0)
            {
                pokemon.AddRange(morePokemon);
                SetPokemonCollection(morePokemon);
            }
            else
            {
                currentPage--;
            }

            IsBusy = false;
        }

        private async Task GoToPokemonDetails()
        {
            if (IsBusy)
                return;

            if (selectedPokemon != null)
            {
                IsBusy = true;
                await NavigateToPage(selectedPokemon.Key);
                IsBusy = false;
            }
        }

        private async Task Scan()
        {
            var photo = await CameraService.TakePhotoAsync();

            IsBusy = true;
            var pokemon = await CustomVisionService.ClassifyImage(photo);

            if (!string.IsNullOrWhiteSpace(pokemon))
            {
                await App.Current.MainPage.DisplayAlert("Pokémon identified!", $"It's {pokemon}! Loading details...", "OK");
                await NavigateToPage(pokemon);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Pokémon not identified!", $"The Pokémon could not be identified. Try again.", "OK");
            }

            IsBusy = false;
        }

        private async Task NavigateToPage(string key)
        {
            var pokemonDetails = await PokemonService.GetPokemonDetails(key, true);
            var vm = new PokemonDetailsViewModel(pokemonDetails);

            var page = new PokemonDetailsView();
            page.BindingContext = vm;

            await App.Current.MainPage.Navigation.PushAsync(page);
        }

        private void SetPokemonCollection(List<MyPokemon> pokemon)
        {
            foreach (var item in pokemon)
                PokemonCollection.Add(item);
        }

        public PokemonListViewModel()
        {
            SelectedPokemon = null;
            PokemonCollection = new ObservableCollection<MyPokemon>();
            LoadPokemonDataCommand = new Command(async () => await LoadPokemonData());
            LoadMorePokemonDataCommand = new Command(async () => await LoadMorePokemonData());
            GoToPokemonDetailsCommand = new Command(async () => await GoToPokemonDetails());
            ScanCommand = new Command(async () => await Scan());
        }
    }
}