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
        public double MusicVolume { get; set; }
        public string GameStatusText { get; set; }

        private int mMultFactor;
        public int MultFactor
        {
            get
            {
                return mMultFactor;
            }
            set
            {
                mMultFactor = value;
                NotifyPropertyChanged("MultFactor");
            }
        }

        public CombatantViewModel LeftCombatantViewModel { get; set; }
        public CombatantViewModel RightCombatantViewModel { get; set; }
        public TriangleFactViewModel QuestionViewModel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string p)
        {
            if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public void SetupBattlers()
        {
            double difficultyFactor = (Challenge - 5) * .1 + 1;
            portraits = new ImageResources(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Portraits\\"));
            Combatant playerCombatant = new Combatant("Mia", 80, 100, 10, 5, "Mia.png");

            LeftCombatantViewModel = new CombatantViewModel(playerCombatant, portraits);

            Combatant EnemyCombatant = GetCombatant(MultFactor, difficultyFactor);
            RightCombatantViewModel = new CombatantViewModel(EnemyCombatant, portraits);

            game = new BattleGame(playerCombatant, EnemyCombatant, 1, 10, 11 - Challenge, new List<int>() { MultFactor }, new List<string>() { "*" });
            game.EnemyPoll += EnemyAct;
            game.EnemyTimerAttack += EnemyTimerAttack;
            game.Victory += Victory;
            game.Defeat += Defeat;

            RightCombatantViewModel.Refresh();
            LeftCombatantViewModel.Refresh();
            CurrentProblem = game.GetProblem();

            QuestionViewModel = new TriangleFactViewModel(CurrentProblem);

            SetGameActive(true);
            
        }

        private  Combatant GetCombatant(int MultFactor, double difficultyFactor)
        {
            Combatant comb;

            switch (MultFactor)
            {
                case 3:
                    comb = new Combatant("Three-Man", 80, 0, (int)(7 * difficultyFactor), 3, "ThreeMan.png");
                    break;
                case 4:
                    comb = new Combatant("Four-Man", 80, 0, (int)(7 * difficultyFactor), 3, "FourMan.png");
                    break;
                default:
                    comb = new Combatant(string.Concat(MultFactor,"-Guy"), 80, 0, (int)(7 * difficultyFactor), 3, "DefaultGuy.png");
                    break;
            }

            return comb;
        }

        public void  SetGameActive(bool gameIsActive)
        {
            game.ActiveGame = gameIsActive;
            GameStatusText = "Battle";

            if(gameIsActive)
            {
                MusicVolume = 0.25;
            }
            NotifyPropertyChanged(string.Empty);
        }

        public BattleTriangleFactViewModel()
        {
            Challenge = 5;
            MultFactor = 5;
            MusicVolume = 0;

            GameStatusText = "Ready To Start...";
            resources = new ResponseResources(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Images\\"), new List<string> { "correct", "wrong", "battle", "bang", "hit" });

            BackgroundResource = resources.GetResponse("battle");
        }

        public void AnswerCurrentProblem(int answer)
        {
            if (game.AttemptProblem(CurrentProblem, answer))
            {
                LeftCombatantViewModel.CombatantData.Attack(RightCombatantViewModel.CombatantData);
                RightCombatantViewModel.Refresh();
                resources.GetResponse("hit").PlaySound();
            }
            else
            {
                // incorrect
                RightCombatantViewModel.CombatantData.Attack(LeftCombatantViewModel.CombatantData);
                LeftCombatantViewModel.Refresh();
                resources.GetResponse("bang").PlaySound();
            }

            CurrentProblem = game.GetProblem();
            QuestionViewModel = new TriangleFactViewModel(CurrentProblem);

            NotifyPropertyChanged(string.Empty);
        }

        public void EnemyAct(object sender, EventArgs e)
        {
            RightCombatantViewModel.Refresh();
            LeftCombatantViewModel.Refresh();
        }

        public void EnemyTimerAttack(object sender, EventArgs e)
        {
            resources.GetResponse("bang").PlaySound();
        }


        public void Victory(object sender, EventArgs e)
        {
            GameStatusText = "Winner!!!";
            MusicVolume = 0;
            NotifyPropertyChanged(string.Empty);
        }

        public void Defeat(object sender, EventArgs e)
        {
            GameStatusText = "Try again next time.";
            MusicVolume = 0;
            NotifyPropertyChanged(string.Empty);
        }

        int mChallenge;
        public int Challenge
        {
            get
            {
                return mChallenge;
            }
            set
            {
                mChallenge = value;

            }
        }
    }
}
