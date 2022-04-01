using System;
using System.Collections.Generic;
using System.Text;

namespace Pokedex.Models
{
    public class MyPokemon
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Sprite { get; set; }
        public List<MyType> Types { get; set; }
        public List<MyAbility> Abilities { get; set; }
        public List<MyMove> Moves { get; set; }
        public MySpecies Species { get; set; }

        public List<MyForm> Forms { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public List<MyStat> Stats { get; set; }

        public string Cry => $"https://play.pokemonshowdown.com/audio/cries/{Name.ToLower()}.mp3";
        public string IdName => $"#{Id:D3} - {Name}";
    }
}
