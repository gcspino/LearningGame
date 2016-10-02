using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningGame.Core;
using System.ComponentModel;

namespace LearningGame.GUI
{
    public class CombatantViewModel : INotifyPropertyChanged
    {
        Combatant mCombatant;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string p)
        {
            if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public CombatantViewModel(Combatant combatant)
        {
            CombatantData = combatant;
        }

        public Combatant CombatantData
        {
            get { return mCombatant; }
            set { mCombatant = value;
                NotifyPropertyChanged(string.Empty);
            }
        }

        public void Refresh()
        {
            NotifyPropertyChanged(string.Empty);
        }
    }
}
