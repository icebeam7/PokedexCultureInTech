using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using PokeApiNet;

using Pokedex.Models;
using System.Text.RegularExpressions;

namespace Pokedex.Services
{
    public static class PokemonService
    {
        private static PokeApiClient pokeClient;

        static PokemonService()
        {
            pokeClient = new PokeApiClient();
        }

        public static async Task<List<MyPokemon>> GetPokemonList(int page, int count)
        {
            var pokemonList = await Task.Run(async () 
                => await GetPokemonListPerPage(page, count));

            return pokemonList;
        }

        public static async Task<MyPokemon> GetPokemonDetails(string pokemonName, bool includeDetails)
        {
            var pokemonDetails = await Task.Run(async ()
                => await GetPokemonData(pokemonName, includeDetails));

            return pokemonDetails;
        }

        private static async Task<List<MyPokemon>> GetPokemonListPerPage(int page, int count)
        {
            var pokemonResources = await pokeClient.GetNamedResourcePageAsync<Pokemon>(count, page * count);

            var pokemonList = new List<MyPokemon>();

            foreach (var pokemonResource in pokemonResources.Results)
            {
                var pokemon = await GetPokemonData(pokemonResource.Name, false);
                pokemonList.Add(pokemon);
            }

            return pokemonList;
        }

        private static async Task<MyPokemon> GetPokemonData(string pokemonName, bool includeDetails)
        {
            var pokemonData = await pokeClient.GetResourceAsync<Pokemon>(pokemonName);
            var pokemonTypes = await GetTypes(pokemonData.Types);
            var stats = await GetStats(pokemonData.Stats);
            var species = await GetSpecies(pokemonData.Species);

            var myPokemon = new MyPokemon()
            {
                Key = pokemonData.Name,
                Name = species.Name,
                Id = pokemonData.Id,
                Types = pokemonTypes,
                Height = pokemonData.Height / 10.0,
                Weight = pokemonData.Weight / 10.0,
                Sprite = pokemonData.Sprites.FrontDefault,
                Stats = stats, 
                Species = species
            };

            if (includeDetails)
            {
                myPokemon.Abilities = await GetAbilities(pokemonData.Abilities);
                myPokemon.Forms = await GetForms(pokemonData.Forms);
                myPokemon.Moves = await GetMoves(pokemonData.Moves);
            }

            return myPokemon;
        }

        private static async Task<List<MyStat>> GetStats(List<PokemonStat> stats)
        {
            var statList = new List<MyStat>();

            foreach (var stat in stats)
            {
                var statData = await pokeClient.GetResourceAsync<Stat>(stat.Stat.Name);

                statList.Add(new MyStat()
                {
                    Base = stat.BaseStat,
                    Name = getName(statData.Names, statData.Name)
                });
            }

            return statList;
        }

        private static async Task<List<MyAbility>> GetAbilities(List<PokemonAbility> abilities)
        {
            var abilityList = new List<MyAbility>();

            foreach (var ability in abilities)
            {
                var abilityData = await pokeClient.GetResourceAsync<Ability>(ability.Ability.Name);
                var descriptionData = abilityData.FlavorTextEntries.FirstOrDefault(x => x.Language.Name == "en");

                abilityList.Add(new MyAbility()
                {
                    Name = getName(abilityData.Names, abilityData.Name),
                    IsHidden = ability.IsHidden,
                    Description = descriptionData?.FlavorText
                });
            }

            return abilityList;
        }

        private static async Task<List<MyForm>> GetForms(List<NamedApiResource<PokemonForm>> forms)
        {
            var formList = new List<MyForm>();

            foreach (var form in forms)
            {
                var formData = await pokeClient.GetResourceAsync<PokemonForm>(form.Name);

                formList.Add(new MyForm()
                {
                    Name = getName(formData.Names, formData.Name),
                    Sprite = formData.Sprites.FrontDefault,
                    IsBattleOnly = formData.IsBattleOnly,
                    IsMega = formData.IsMega
                });
            }

            return formList;
        }

        private static async Task<List<MyMove>> GetMoves(List<PokemonMove> moves)
        {
            var moveList = new List<MyMove>();

            foreach (var move in moves)
            {
                var moveData = await pokeClient.GetResourceAsync<Move>(move.Move.Name);
                var descriptionData = moveData.FlavorTextEntries.FirstOrDefault(x => x.Language.Name == "en");
                var myType = await GetType(moveData.Type);

                moveList.Add(new MyMove()
                {
                    Accuracy = getValue(moveData.Accuracy),
                    Power = getValue(moveData.Power),
                    PP = getValue(moveData.Pp),
                    Name = getName(moveData.Names, moveData.Name),
                    DamageClass = moveData.DamageClass.Name,
                    Description = descriptionData?.FlavorText,
                    Type = myType,
                });
            }

            return moveList;
        }

        private static async Task<List<MyType>> GetTypes(List<PokemonType> types)
        {
            var typeList = new List<MyType>();

            foreach (var type in types)
            {
                var myType = await GetType(type.Type);
                typeList.Add(myType);
            }

            return typeList;
        }

        private static async Task<MyType> GetType(NamedApiResource<Type> type)
        {
            var typeData = await pokeClient.GetResourceAsync<Type>(type.Name);

            return new MyType()
            {
                Name = getName(typeData.Names, typeData.Name)
            };
        }

        private static async Task<MySpecies> GetSpecies(NamedApiResource<PokemonSpecies> species)
        {
            var speciesData = await pokeClient.GetResourceAsync<PokemonSpecies>(species.Name);
            var descriptionData = speciesData.FlavorTextEntries.FirstOrDefault(x => x.Language.Name == "en");

            var description = descriptionData?.FlavorText.Replace('\n', ' ');

            return new MySpecies()
            {
                Name = getName(speciesData.Names, speciesData.Name),
                Description = description
            };
        }

        private static string getName(List<Names> names, string defaultValue)
        {
            if (names.Count > 0)
            {
                var nameData = names.FirstOrDefault(x => x.Language.Name == "en");
                return nameData.Name;
            }
            else
                return defaultValue;
        }

        private static int getValue(int? data) => data.HasValue ? data.Value : 0;
    }
}
