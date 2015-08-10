using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication3.Abilities;
using ConsoleApplication3.Conditions;
using ConsoleApplication3.Targets;

namespace ConsoleApplication3
{

    class Program
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
                    AttackDamage = level.rnd.Next(5, 15),
                    HP = level.rnd.Next(75, 125),
                    APRechargeRate = level.rnd.Next(7, 12),
                    IsAlive = true
                };
                ch.MaxHP = ch.HP;
                ch.ActionList.Add(Action.AttackNearestPlayer);
                ch.ActionList.Add(Action.HealSelf);
                enemies.Add(ch);
            }

            enemies[enemies.Count - 1].ActionList.Insert(0, new Action()
            {
                Ability = new AbilityHeal(),
                Condition = new ConditionHPLessThan(75),
                Target = new TargetAliveEnemy(),
            });
            level.enemy = new Party(enemies.ToArray());

            var players = new List<Character>();
            for (var i = 0; i < 3; i++)
            {
                var ch = new Character()
                {
                    Name = "Player " + i,
                    AttackDamage = level.rnd.Next(7, 17),
                    HP = level.rnd.Next(175, 275),
                    APRechargeRate = level.rnd.Next(7, 12),
                    IsAlive = true,
                };
                ch.MaxHP = ch.HP;
                ch.ActionList.Add(Action.AttackNearestEnemy);
                ch.ActionList.Add(Action.HealSelf);
                players.Add(ch);
            }

            players[players.Count - 1].ActionList.Insert(0, new Action()
            {
                Ability = new AbilityHeal(),
                Condition = new ConditionHPLessThan(75),
                Target = new TargetAlivePlayer(),
            });
            level.player = new Party(players.ToArray());



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
    }
}