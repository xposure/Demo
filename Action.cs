using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication3.Abilities;
using ConsoleApplication3.Conditions;

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
}

