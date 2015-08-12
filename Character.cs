using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleApplication3
{
    public class Character
    {
        public Faction Faction;
        public CharacterGroup Party;

        public string Name;
        public Stats Stats = new Stats();

        public Vector2 Position;

        //public int Distance;
        public float AP;
        public string LastAction = string.Empty;
        public bool IsAlive;

        public Character currentTarget;
        //private Action2 currentAction;

        private int currentAction = -1;
        public List<Action2> ActionList = new List<Action2>();

        public void Turn(Level level, float dt)
        {
            //check if ability is still valid
            if (currentAction > -1 && !ActionList[currentAction].Ability.CanUse(level, this, currentTarget))
            {
                //cancel
                currentAction = -1;
                AP = 0;
                currentTarget = null;
            }

            //will check all actions or the 1 up to the currentAction (for canceling)
            var maxAction = currentAction == -1 ? ActionList.Count : currentAction;            
            for (var i = 0; i < maxAction; i++)
            {
                //found either a new action to run or higher priority action
                if (ActionList[i].Check(level, this))
                {
                    if (currentAction > -1)
                    {
                        //cancel (don't clear target because Check set the new one)
                    }

                    currentAction = i;
                    AP = 0;
                    break;
                }
            }

            if (currentAction > -1)
            {
                LastAction = ActionList[currentAction].Ability.GetType().Name + " " + currentTarget.Name;
                AP += ActionList[currentAction].Ability.ChargeRate * APRechargeRate * dt;
                if (AP >= 100)
                {
                    ActionList[currentAction].Ability.Use(level, this, currentTarget);
                    AP = 0;
                    currentTarget = null;
                    currentAction = -1;
                    Turn(level, dt);
                }
            }
            else
            {
                LastAction = "Idle";
            }
        }

        public void TakeDamage(int damage)
        {
            if (IsAlive)
            {
                HP -= damage;
                if (HP < 0)
                {
                    //Console.WriteLine("{0} has died!", Name);
                    IsAlive = false;
                }
            }
        }

        public void HealDamage(int damage)
        {
            if (IsAlive)
            {
                HP += damage;
                if (HP > MaxHP)
                    HP = MaxHP;
            }
        }

        public float HP
        {
            get { return Stats.GetBaseValue("hp_now"); }
            set { Stats.SetBaseStat("hp_now", value); }
        }

        public float MaxHP {
            get { return Stats.GetBaseValue("hp_max"); }
            //set { Stats.SetBaseStat("hp_max", value); }
        }

        public float AttackDamage
        {
            get { return Stats.GetModifiedValue("attack_damage") * 2; }
            //set { Stats.SetBaseStat("attack_damage", value);}
        }

        public float APRechargeRate { get { return Stats.GetModifiedValue("speed"); } } 

        public int APOver10 { get { return (int)(AP / 10); } }

        public int HPPercent { get { return (int)(100 * ((float)HP / (float)MaxHP)); } }

        public int Size { get { return 10; } }

        public float Left { get { return Position.X - Size / 2; } }
        public float Right { get { return Position.X + Size / 2; } }
        public float Top { get { return Position.Y + Size / 2; } }
        public float Bottom { get { return Position.Y - Size / 2; } }

        public bool IsAlly(Character other)
        {
            return !Faction.IsHostile(other.Faction);
        }

        public void Draw(SpriteBatch batch)
        {
            if (IsAlive)
            {
                var color = Faction == Faction.Ally ? Color.Blue : Color.Red;
                if (currentTarget != null)
                {
                    var dir = currentTarget.Position - Position;
                    //dir.Normalize();
                    dir *= (AP / 100);

                    batch.DrawLine(Position, Position + dir, color, 2);
                }

                batch.DrawRect(Position, 20, color);
            }
        }

        public override string ToString()
        {
            if (IsAlive)
                return string.Format("{0,10} - HP: {1,3} {7,3}%, AD: {2,3}, AR: {6,2}, AP: [{3}{4}] {5}", Name, HP, AttackDamage, new string('#', APOver10), new string(' ', 10 - APOver10), LastAction, APRechargeRate, HPPercent);

            return string.Format("{0,10} - Dead!", Name);
        }
    }
}
