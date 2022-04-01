using System;
using System.Collections.Generic;
using System.Text;

namespace Pokedex.Models
{
    public class MyMove
    {
        public string Name { get; set; }
        public string DamageClass { get; set; }
        public int Accuracy { get; set; }
        public string Description { get; set; }
        public int Power { get; set; }
        public int PP { get; set; }
        public MyType Type { get; set; }
    }
}
