using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication3.Abilities;
using ConsoleApplication3.Conditions;
using ConsoleApplication3.Targets;

namespace ConsoleApplication3
{
    public class Action
    {
        public Target Target;
        public Condition Condition;
        public Ability Ability;

        public bool Perform(Character actor, Level level)
        {
            var targets = Target.FindTargets(level, actor);

            foreach (var target in targets)
            {

                if (!Condition.Check(level, target))
                    continue;

                if (!Ability.Use(level, actor, target))
                    continue;

                return true;
            }

            return false;
        }

        public static readonly Action AttackNearestEnemy = new Action()
        {
            Target = new TargetAliveEnemy(),
            Condition = new ConditionAny(),
            Ability = new AbilityAttack()
        };

        public static readonly Action AttackNearestPlayer = new Action()
        {
            Target = new TargetAlivePlayer(),
            Condition = new ConditionAny(),
            Ability = new AbilityAttack()
        };

        public static readonly Action HealSelf = new Action()
        {
            Target = new TargetSelf(),
            Condition = new ConditionHPLessThan(90),
            Ability = new AbilityHeal()
        };
    }

}
