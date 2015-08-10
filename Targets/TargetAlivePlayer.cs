using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Targets
{
    public class TargetAlivePlayer : Target
    {
        public override IEnumerable<Character> FindTargets(Level level, Character actor)
        {
            foreach (var player in level.player)
                if (player.IsAlive)
                    yield return player;
        }
    }
}
