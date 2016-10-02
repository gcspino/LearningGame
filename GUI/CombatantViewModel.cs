using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningGame.Core;
using System.ComponentModel;
using System.Windows.Media.Imaging;

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

        public CombatantViewModel(Combatant combatant, ImageResources portraits)
        {
            CombatantData = combatant;
            Image = portraits.GetImage(combatant.PortraitName);
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

        private BitmapImage mImage;
        public BitmapImage Image
        {
            get
            {
                return mImage;
            }

            set
            {
                mImage = value;
                NotifyPropertyChanged("Image");
            }
        }
    }
}
