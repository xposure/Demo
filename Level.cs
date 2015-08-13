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
        //public CharacterGroup enemy;
        //public CharacterGroup player;
        public List<Character> allCharacters = new List<Character>();

        public IEnumerable<Character> Characters
        {
            get
            {
                return allCharacters;
                //foreach (var e in enemy)
                //    yield return e;

                //foreach (var p in player)
                //    yield return p;
            }
        }

        public IEnumerable<Character> Allies
        {
            get { return Characters.Where(x => x.Faction == Faction.Ally); }
        }

        public IEnumerable<Character> IsAlly(Character actor)
        {
            return Characters.Where(x => !x.Faction.IsHostile(actor.Faction));
        }

        public IEnumerable<Character> Foes
        {
            get { return Characters.Where(x => x.Faction == Faction.Foe); }
        }

        public IEnumerable<Character> IsFoe(Character actor)
        {
            return Characters.Where(x => x.Faction.IsHostile(actor.Faction));
        }
    }
}
