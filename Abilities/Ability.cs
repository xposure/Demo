using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Abilities
{
    public class Ability
    {
        public virtual bool Use(Level level, Character actor, Character target)
        {
            return false;
        }
    }
}
