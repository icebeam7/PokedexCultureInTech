using Pokedex.Models;

namespace Pokedex.ViewModels
{
    public class PokemonDetailsViewModel : BaseViewModel
    {
        private MyPokemon selectedPokemon;

        public MyPokemon SelectedPokemon
        {
            get { return selectedPokemon; }
            set { SetProperty(ref selectedPokemon, value); }
        }

        public PokemonDetailsViewModel(MyPokemon selectedPokemon)
        {
            SelectedPokemon = selectedPokemon;
        }
    }
}
