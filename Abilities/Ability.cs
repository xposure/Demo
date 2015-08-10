using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Abilities
{
    public class Ability
    {
        public float ChargeRate = 10;

        public virtual bool CanUse(Level level, Character actor, Character target)
        {
            return false;
        }

        public virtual void Use(Level level, Character actor, Character target)
        {
        }
    }
}
