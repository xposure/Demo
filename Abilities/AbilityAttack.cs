using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Abilities
{
    public class AbilityAttack : Ability
    {

        public override string Name
        {
            get
            {
                return "Attack";
            }
        }

        public override bool CanUse(Level level, Character actor, Character target)
        {
            return target != null && target.IsAlive;
        }

        public override void Use(Level level, Character actor, Character target)
        {
            //Console.WriteLine("{0} is attacking {1} for {2} damage!", actor.Name, target.Name, actor.AttackDamage);
            var damage = (int)(actor.AttackDamage + level.rnd.Next(-2, 2));
            target.TakeDamage(damage);
            //actor.LastAction = string.Format("Attacked {0} for {1} damage!", target.Name, damage);
        }
    }
}
