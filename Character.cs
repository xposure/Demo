using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public class Character
    {
        public string Name;
        public int HP;
        public int MaxHP;
        public int AttackDamage;
        public int AP;
        public int APRechargeRate;
        public string LastAction = "Charging";
        public bool IsAlive;

        public List<Action> ActionList = new List<Action>();

        public void Turn(Level level)
        {
            AP += APRechargeRate;
            if (AP > 100)
            {
                AP = 100;
                LastAction = "Idle";
            }

            if (AP == 100)
            {
                foreach (var action in ActionList)
                {
                    if (action.Perform(this, level))
                    {
                        AP = 0;
                        break;
                    }
                }
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

        public int APOver10 { get { return AP / 10; } }

        public int HPPercent { get { return (int)(100 * ((float)HP / (float)MaxHP)); } }

        public override string ToString()
        {
            if(IsAlive)
                return string.Format("{0,10} - HP: {1,3} {7,3}%, AD: {2,3}, AR: {6,2}, AP: [{3}{4}] {5}", Name, HP, AttackDamage, new string('#', APOver10), new string(' ', 10 - APOver10), LastAction, APRechargeRate, HPPercent);

            return string.Format("{0,10} - Dead!", Name);
        }
    }
}
