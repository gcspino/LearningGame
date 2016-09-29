using LearningGame.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;

namespace LearningGame.GUI
{
    public class BattleTriangleFactViewModel : INotifyPropertyChanged
    {
        private BattleGame game;

        private ResponseResources resources;
        private ResponseUIPair BackgroundResource;

        public Problem CurrentProblem { get; set; }

        public CombatantViewModel LeftCombatantViewModel { get; set; }
        public CombatantViewModel RightCombatantViewModel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string p)
        {
            if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public BattleTriangleFactViewModel()
        {
            Combatant playerCombatant = new Combatant("Mia", 80, 10, 5);

            LeftCombatantViewModel = new CombatantViewModel(playerCombatant);

            Combatant EnemyCombatant = new Combatant("Goblin", 50, 7, 3);
            RightCombatantViewModel = new CombatantViewModel(EnemyCombatant);

            game = new BattleGame(playerCombatant, EnemyCombatant, 0, 10, new List<string>() { "+", "-" });
            CurrentProblem = game.Problems[0];
            NotifyPropertyChanged(string.Empty);
            resources = new ResponseResources(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Images\\"), new List<string> { "correct", "wrong", "battle" });
            BackgroundResource = resources.GetResponse("battle");
        }

        public void AnswerCurrentProblem(int answer)
        {
            if (game.AttemptProblem(CurrentProblem, answer))
            {
                // correct

            }
            else
            {
                // incorrect
            }


            CurrentProblem = game.GetProblem();

            NotifyPropertyChanged(string.Empty);
        }

    }
}
