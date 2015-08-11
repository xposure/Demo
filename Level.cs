using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public class Level
    {
        public Random rnd = new Random();
        public CharacterGroup enemy;
        public CharacterGroup player;

        public IEnumerable<Character> Characters
        {
            get
            {
                foreach (var e in enemy)
                    yield return e;

                foreach (var p in player)
                    yield return p;
            }
        }

        public IEnumerable<Character> Allies(Character actor)
        {
            return Characters.Where(x => !x.Faction.IsHostile(actor.Faction));
        }

        public IEnumerable<Character> Foes(Character actor)
        {
            return Characters.Where(x => x.Faction.IsHostile(actor.Faction));
        }
    }
}
