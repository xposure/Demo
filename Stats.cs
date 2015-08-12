using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public class StatModifier
    {
        public int ID;
        public float FlatAmount;
        public float PercentAmount;
    }

    //public class DerivedStat
    //{
    //    public abstract float GetValue(Stats stats);
    //}

    //public class MaxHPStat : DerivedStat
    //{
    //    public override float GetValue(Stats stats)
    //    {
    //        return stats.GetModifiedValue("vitality") * 15;
    //    }
    //}

    //public class AttackStat : DerivedStat
    //{
    //    public override float GetValue(Stats stats)
    //    {
    //        return stats.GetModifiedValue("strength") * 5;
    //    }
    //}

    //public class DefenseStat : DerivedStat
    //{
    //    public override float GetValue(Stats stats)
    //    {
    //        return stats.GetModifiedValue("vitality") * 2;
    //    }
    //}

    public class Stats
    {
        private static readonly Random rand = new Random();
        private Dictionary<string, float> baseStats = new Dictionary<string, float>();
        private Dictionary<string, float> modifiedStats = new Dictionary<string, float>();
        private Dictionary<string, List<StatModifier>> statModifiers = new Dictionary<string, List<StatModifier>>();
        private HashSet<string> modifiedStatDirty = new HashSet<string>();
        //private Dictionary<string, DerivedStat> derivedStat = new Dictionary<string, DerivedStat>();

        public Stats()
        {

        }

        public float AddToBaseState(string name, float amount)
        {
            var value = baseStats[name];
            value += amount;
            baseStats[name] = value;
            modifiedStatDirty.Add(name);

            return value;
        }

        public void SetBaseStat(string name, float amount)
        {
            baseStats[name] = amount;
            modifiedStatDirty.Add(name);
        }

        public void AddBaseStat(string name, float amount)
        {
            baseStats[name] = amount;
            statModifiers.Add(name, new List<StatModifier>());
            modifiedStats[name] = amount;
        }

        public float AddBaseStat(string name, int dice, int sides)
        {
            var result = 0f;
            for (var d = 0; d < dice; ++d)
                result += (float)Math.Floor(rand.NextFloat() * sides) + 1;

            baseStats[name] = result;
            modifiedStats[name] = result;
            statModifiers.Add(name, new List<StatModifier>());
            return result;
        }

        public float GetBaseValue(string name)
        {
            return baseStats[name];
        }

        public float GetModifiedValue(string name)
        {
            if (modifiedStatDirty.Contains(name))
                UpdateModifier(name);

            return modifiedStats[name];
        }

        public void AddModifier(string name, int id, float flat, float percent)
        {
            var list = statModifiers[name];
            if (list.Any(x => x.ID == id))
                return;

            list.Add(new StatModifier() { ID = id, FlatAmount = flat, PercentAmount = percent });
            modifiedStatDirty.Add(name);
        }

        public void RemoveModifier(string name, int id)
        {
            var list = statModifiers[name];
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].ID == id)
                    list.RemoveAt(i);
            }
        }

        private void UpdateModifier(string name)
        {
            var value = baseStats[name];
            var flat = 0f;
            var percent = 0f;
            var list = statModifiers[name];

            foreach (var mod in list)
            {
                flat += mod.FlatAmount;
                percent += mod.PercentAmount;
            }

            value += flat;
            value += value * percent;
            modifiedStats[name] = value;
            modifiedStatDirty.Remove(name);
        }

        public void AddDerivedStat(string name)
        {

        }
    }
}
