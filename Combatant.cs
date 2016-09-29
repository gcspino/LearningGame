using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningGame.Core
{
    public class Combatant
    {
        Random rnd = new Random();

        public Combatant(string name, int maxHP, int physicalAttack, int physicalDefense)
        {
            Name = name;
            MaxHP = maxHP;
            PhysicalAttack = physicalAttack;
            PhysicalDefense = physicalDefense;
        }

        public string Name { get; set; }
        public int MaxHP { get; set; }
        private int mCurrentHP;
        public int CurrentHP
        {
            get { return mCurrentHP; }
            set
            {
                mCurrentHP -= Math.Min(value, mCurrentHP);
            }
        }

        public int PhysicalAttack { get; set; }
        public int PhysicalDefense { get; set; }

        public void Attack(Combatant target)
        {
            int attackRoll = (int) Math.Round(rnd.Next(70, 130) * (double) PhysicalAttack / 100);
            int damage = attackRoll - target.PhysicalDefense;
            target.CurrentHP -= damage;
        }

    }
}
