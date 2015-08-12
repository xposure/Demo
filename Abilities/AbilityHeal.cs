using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Abilities
{
    public class AbilityHeal : Ability
    {
        public override bool CanUse(Level level, Character actor, Character target)
        {
            return target.IsAlive;
        }

        public override void Use(Level level, Character actor, Character target)
        {
            var damage = (int)(actor.AttackDamage + level.rnd.Next(-2, 2)) * 4;
            target.HealDamage(damage);
            //actor.LastAction = string.Format("Healed {0} for {1} damage!", target.Name, damage);
        }
    }
}
