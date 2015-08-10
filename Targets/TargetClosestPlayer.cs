using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Targets
{
    public class TargetClosestPlayer : TargetAlivePlayer
    {
        public override IEnumerable<Character> FindTargets(Level level, Character actor)
        {
            return base.FindTargets(level, actor).OrderBy(x => (Math.Abs(actor.Distance - x.Distance)));
        }
    }
}
