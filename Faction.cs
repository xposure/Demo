using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public class Faction
    {
        public string Name;
        public bool IsHostile(Faction other)
        {
            return this != other;
        }


        public static readonly Faction Ally = new Faction() { Name = "Ally" };
        public static readonly Faction Foe = new Faction() { Name = "Foe" };

       
    }
}
