using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Conditions
{
    public class ConditionAny : Condition
    {
        public override bool Check(Level level, Character target)
        {
            return target != null;
        }
    }
}
