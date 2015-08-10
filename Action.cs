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

    public class Action2
    {
        public Condition2 Condition;
        public Ability Ability;

        public Action2(Condition2 condition, Ability ability)
        {
            Condition = condition;
            Ability = ability;
        }

        public bool Check(Level level, Character actor)
        {
            return Condition(level, actor, Ability);
        }
    }

    public class Action
    {
        public Target Target;
        public Condition Condition;
        public Ability Ability;

        public Character FindValidTarget(Level level, Character actor)
        {
            var targets = Target.FindTargets(level, actor);
            return targets.FirstOrDefault(x => Check(level, actor, x));
        }

        public bool Check(Level level, Character actor, Character target)
        {
            return Condition.Check(level, target) && Ability.CanUse(level, actor, target);
        }

        public static readonly Action AttackNearestEnemy = new Action()
        {
            Target = new TargetClosestEnemy(),
            Condition = new ConditionAny(),
            Ability = new AbilityAttack()
        };

        public static readonly Action AttackNearestPlayer = new Action()
        {
            Target = new TargetClosestPlayer(),
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

