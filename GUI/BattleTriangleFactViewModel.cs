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
        private ImageResources portraits;
        private ResponseUIPair BackgroundResource;

        public Problem CurrentProblem { get; set; }

        public CombatantViewModel LeftCombatantViewModel { get; set; }
        public CombatantViewModel RightCombatantViewModel { get; set; }
        public TriangleFactViewModel QuestionViewModel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string p)
        {
            if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public BattleTriangleFactViewModel()
        {
            portraits = new ImageResources(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Portraits\\"));
            Combatant playerCombatant = new Combatant("Mia", 80, 10, 5, "Mia.png");

            LeftCombatantViewModel = new CombatantViewModel(playerCombatant, portraits);

            Combatant EnemyCombatant = new Combatant("Goblin", 50, 7, 3, "Witch.png");
            RightCombatantViewModel = new CombatantViewModel(EnemyCombatant, portraits);

            game = new BattleGame(playerCombatant, EnemyCombatant, 1, 10, new List<string>() {  "*", "/" });
            CurrentProblem = game.GetProblem();

            QuestionViewModel = new TriangleFactViewModel(CurrentProblem);

            NotifyPropertyChanged(string.Empty);
            resources = new ResponseResources(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Images\\"), new List<string> { "correct", "wrong", "battle" });

            BackgroundResource = resources.GetResponse("battle");
        }

        public void AnswerCurrentProblem(int answer)
        {
            if (game.AttemptProblem(CurrentProblem, answer))
            {
                LeftCombatantViewModel.CombatantData.Attack(RightCombatantViewModel.CombatantData);
                RightCombatantViewModel.Refresh();
            }
            else
            {
                // incorrect
                RightCombatantViewModel.CombatantData.Attack(LeftCombatantViewModel.CombatantData);
                LeftCombatantViewModel.Refresh();
            }


            CurrentProblem = game.GetProblem();
            QuestionViewModel = new TriangleFactViewModel(CurrentProblem);

            NotifyPropertyChanged(string.Empty);
        }

    }
}
