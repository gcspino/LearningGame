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

        public Combatant(string name, int maxHP, int maxMana, int physicalAttack, int physicalDefense, string portraitName)
        {
            Name = name;
            MaxHP = maxHP;
            CurrentHP = MaxHP;
            MaxMana = maxMana;
            CurrentMana = MaxMana;
            PhysicalAttack = physicalAttack;
            PhysicalDefense = physicalDefense;
            PortraitName = portraitName;
        }

        public string PortraitName { get; set;}

        public string Name { get; set; }
        public int MaxHP { get; set; }

        public int MaxMana { get; set; }
        public int CurrentMana { get; set; }
        public Action EmptyBag = null;

        public Action Pulse = null;

        private int mCurrentHP;
        public int CurrentHP
        {
            get { return mCurrentHP; }
            set
            {
                mCurrentHP = value;
                if(mCurrentHP <= 0)
                {
                    mCurrentHP = 0;
                    OnDefeat(EventArgs.Empty);
                }
            }
        }

        public int PhysicalAttack { get; set; }
        public int PhysicalDefense { get; set; }

        public void Attack(Combatant target)
        {
            if (PhysicalAttack > 0)
            {
                int attackRoll = (int)Math.Round(rnd.Next(70, 130) * (double)PhysicalAttack / 100);
                int damage = attackRoll - target.PhysicalDefense;
                damage = damage < 0 ? 1 : damage;
                target.CurrentHP -= damage;
            }
        }

        protected virtual void OnDefeat(EventArgs e)
        {
            EventHandler handler = Defeat;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler Defeat;

    }
}
