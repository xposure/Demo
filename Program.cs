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
        static void Main(string[] args)
        {
            var level = new Level();

            var enemies = new List<Character>();
            for (var i = 0; i < 5; i++)
            {
                var ch = new Character()
                {
                    Name = "Enemy " + i,
                    Faction = Faction.Foe,
                    Distance = level.rnd.Next(10, 100),
                    AttackDamage = level.rnd.Next(5, 15),
                    HP = level.rnd.Next(75, 125),
                    APRechargeRate = level.rnd.NextFloat(0.8f, 1.2f),
                    IsAlive = true
                };
                ch.MaxHP = ch.HP;
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
                var ch = new Character()
                {
                    Name = "Player " + i,
                    Faction = Faction.Ally,
                    Distance = level.rnd.Next(10, 100),
                    AttackDamage = level.rnd.Next(7, 17),
                    HP = level.rnd.Next(175, 275),
                    APRechargeRate = level.rnd.NextFloat(0.8f, 1.2f),
                    IsAlive = true,
                };
                ch.MaxHP = ch.HP;
                ch.ActionList.Add(new Action2(Condition.FoeNearest, new AbilityAttack()));
                //ch.ActionList.Add(Action.HealSelf);
                players.Add(ch);
            }

            players[0].APRechargeRate = 1.5f;
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

                System.Threading.Thread.Sleep(100);
            }
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }
    }
}