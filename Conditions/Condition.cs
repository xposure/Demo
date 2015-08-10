using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Conditions
{
    public class Condition
    {
        public virtual bool Check(Level level, Character target)
        {
            return false;
        }
    }
}
