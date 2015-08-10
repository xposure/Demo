using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3.Targets
{
    public class TargetAliveEnemy : Target
    {
        public override IEnumerable<Character> FindTargets(Level level, Character actor)
        {
            foreach (var enemy in level.enemy)
                if (enemy.IsAlive)
                    yield return enemy;

        }
    }
}
