using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Targets
{
    public class Target
    {
        public virtual IEnumerable<Character> FindTargets(Level level, Character actor)
        {
            return null;
        }
    }
}
