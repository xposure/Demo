using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Targets
{
    public class TargetSelf : Target
    {
        public override IEnumerable<Character> FindTargets(Level level, Character actor)
        {
            yield return actor;
        }
    }
}
