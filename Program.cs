using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication3.Abilities;
using ConsoleApplication3.Conditions;

namespace ConsoleApplication3
{
    /*
     * Mechanics
     *  - Action system like FF12
     *  - 
     * 
     */
    //http://blog.gambrinous.com/2012/12/04/seabird-plunge-a-flash-game-with-source/

    public static class Program
    {
        static Random rand = new Random();
        static void Main(string[] args)
        {
            var level = new Level();

            var enemies = new List<Character>();
            for (var i = 0; i < 5; i++)
            {
                var ch = CreateEnemy("Enemy " + i);

                ch.ActionList.Add(new Action2(Condition.FoeNearest, new AbilityAttack()));
                //ch.ActionList.Add(Action.HealSelf);
                enemies.Add(ch);
            }

            //enemies[enemies.Count - 1].ActionList.Insert(0, new Action()
            //{
            //    Ability = new AbilityHeal(),
            //    Condition = new ConditionHPLessThan(75),
            //    Target = new TargetAliveEnemy(),
            //});
            level.enemy = new CharacterGroup(enemies.ToArray());

            var players = new List<Character>();
            for (var i = 0; i < 3; i++)
            {
                var ch = CreatePlayer("Player " + i);
                if (i > 0)
                    ch.ActionList.Add(new Action2(Condition.FoePartyLeaderTarget, new AbilityAttack()));

                ch.ActionList.Add(new Action2(Condition.FoeNearest, new AbilityAttack()));
                //ch.ActionList.Add(Action.HealSelf);
                players.Add(ch);
            }

            //players[0].APRechargeRate = 1.5f;
            players[0].ActionList.Add(new Action2(Condition.AllyAnyLessThan90, new AbilityHeal()));
            level.player = new CharacterGroup(players.ToArray());


            var line = string.Empty;
            while (!Console.KeyAvailable)
            {
                System.Console.Clear();
                //Console.WriteLine();

                foreach (var enemy in enemies)
                {
                    if (enemy.IsAlive)
                        enemy.Turn(level);
                }

                foreach (var player in players)
                {
                    if (player.IsAlive)
                        player.Turn(level);
                }

                //Console.WriteLine();

                foreach (var enemy in enemies)
                    //if (enemy.IsAlive)
                        Console.WriteLine(enemy);

                foreach (var player in players)
                    //if (player.IsAlive)
                        Console.WriteLine(player);

                System.Threading.Thread.Sleep(500);
            }
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }

        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        public static Character CreateEnemy(string name)
        {
            var ch = new Character()
            {
                Name = name,
                Faction = Faction.Foe,
                Distance = rand.Next(10, 100),
                IsAlive = true
            };

            var stats = ch.Stats;
            var hp = stats.AddBaseStat("hp_now", 50, 3);
            stats.AddBaseStat("hp_max", hp);

            var mp = stats.AddBaseStat("mp_now", 25, 3);
            stats.AddBaseStat("mp_max", mp);

            stats.AddBaseStat("attack_damage", 3, 3);
            stats.AddBaseStat("strength", 5, 3);
            stats.AddBaseStat("vitality", 5, 3);
            stats.AddBaseStat("intellect", 5, 3);
            stats.AddBaseStat("mind", 5, 3);
            stats.AddBaseStat("speed", 3, 2);
            
            return ch;
        }


        public static Character CreatePlayer(string name)
        {
            var ch = new Character()
            {
                Name = name,
                Faction = Faction.Ally,
                Distance = rand.Next(10, 100),
                IsAlive = true
            };

            var stats = ch.Stats;
            var hp = stats.AddBaseStat("hp_now", 70, 3);
            stats.AddBaseStat("hp_max", hp);

            var mp = stats.AddBaseStat("mp_now", 35, 3);
            stats.AddBaseStat("mp_max", mp);

            stats.AddBaseStat("attack_damage", 4, 3);
            stats.AddBaseStat("strength", 5, 3);
            stats.AddBaseStat("vitality", 5, 3);
            stats.AddBaseStat("intellect", 5, 3);
            stats.AddBaseStat("mind", 5, 3);
            stats.AddBaseStat("speed", 3, 2);

            return ch;
        }
    }
}