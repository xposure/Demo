using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Conditions
{
    public class ConditionHPLessThan : Condition
    {
        public int Percent;
        public ConditionHPLessThan(int percent)
        {
            Percent = percent;
        }

        public override bool Check(Level level, Character target)
        {
            return (target.HPPercent < Percent);
        }
    }
}
