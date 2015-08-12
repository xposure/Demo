using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication3.Abilities;

namespace ConsoleApplication3.Conditions
{
    public delegate bool Condition2(Level level, Character actor, Ability ability);

    public static class Condition
    {
        /*
         * Ally: Any                    -- check each ability can use, applies to all alies at all times
         * Ally: Strongest Weapon       -- check each ally for best weapon, applies to only 1 ally
         * Ally: HP < XX%              -- check each ally for low hp, prioritize not being cured, prioritize lowest
         * Ally: Status <effect>        -- check each ally for effect, only cast if not being cured by another ally
         * Ally: Specific               -- only applies to one
         * 
         * Foe: Party leader's target   -- Do we have a party leader target? Can we use ability?
         * Foe: Highest HP              -- check each foe in range and part of the encounter?
         * 
         * Self: MP < XX%               -- Are we less than X percent of mana? can we use ability?
         * Self: Item Amt > X           -- Do we have more than X items? Does it apply status effect? Do we already have status effect?
         *          
         * Abilities: Cure              -- have enough mp? are we silenced?
         * Abilities: Attack            -- are we close enough? can we attack?
         * Abilities: Haste             -- have enough mp? already have haste?
         */

        public static readonly Condition2 AllyAny = (l, c, a) => Any(l, c, a, l.Allies(c));
        public static readonly Condition2 AllyAnyLessThan50 = (l, c, a) => Any(l, c, a, l.Allies(c).Where(x => x.HPPercent < 50));
        public static readonly Condition2 FoeAny = (l, c, a) => Any(l, c, a, l.Foes(c));
        public static readonly Condition2 FoeNearest = (l, c, a) => Any(l, c, a, l.Foes(c).OrderBy(x => (c.Position - x.Position).LengthSquared()));

        public static bool FoePartyLeaderTarget(Level level, Character actor, Ability ability)
        {
            var leader = level.player.PartyLeader;
            if (leader == null)
                return false;

            var target = leader.currentTarget;
            if (target == null)
                return false;

            if (!target.Faction.IsHostile(actor.Faction))
                return false;

            if (ability.CanUse(level, actor, target))
            {
                actor.currentTarget = target;   
                return true;
            }

            return false;
        }

        public static bool Any(Level level, Character actor, Ability ability, IEnumerable<Character> targets)
        {
            foreach (var target in targets)
            {
                if (target != null && ability.CanUse(level, actor, target))
                {
                    actor.currentTarget = target;
                    return true;
                }
            }

            return false;
        }
    }
}
