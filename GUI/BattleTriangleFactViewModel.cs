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

        private string mMultFactor;
        public string MultFactor
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

        public List<string> Operators
        {
            get
            {
                return new List<string>() { "x", "+", "-" };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string p)
        {
            if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public List<int> GetMultFactors(string multFactors)
        {
            int singleFactor = 0;
            if(int.TryParse(multFactors, out singleFactor))
            {
                return new List<int>() { singleFactor };
            }
            else
            {
                string[] splitFactors= multFactors.Split(new string[] { "," , ";", " " }, StringSplitOptions.RemoveEmptyEntries);
                if (splitFactors.Any())
                {
                    List<int> factors = new List<int>();
                    foreach (string subFactor in splitFactors)
                    {
                        if (int.TryParse(subFactor, out singleFactor))
                        {
                            factors.Add(singleFactor);
                        }
                    }

                    if(!factors.Any())
                    {
                        return null;
                    }

                    return factors;
                }
                else
                {
                    return null;
                } 
            }
        }

        public void SetupBattlers()
        {
            if (game != null)
            {
                game.ActiveGame = false;
                game = null;
            }
                double difficultyFactor = (Challenge - 5) * .1 + 1;
            portraits = new ImageResources(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Portraits\\"));
            Combatant playerCombatant = new Combatant("Mia", 80, 100, 10, 5, "Mia.png");

            LeftCombatantViewModel = new CombatantViewModel(playerCombatant, portraits);
            List<int> multFactors = GetMultFactors(mMultFactor);

            Combatant EnemyCombatant = GetCombatant(multFactors.First(), difficultyFactor, mBossMode, playerCombatant);
            RightCombatantViewModel = new CombatantViewModel(EnemyCombatant, portraits);

            if (mBossMode)
            {
                EnemyCombatant.EmptyBag += () =>
                {
                    LeftCombatantViewModel.Refresh();
                    RightCombatantViewModel.Refresh();
                };
                if(Operator != "x")
                {
                    EnemyCombatant.CurrentHP = 15;
                    RightCombatantViewModel.Refresh();
                }
                playerCombatant.CurrentMana = 0;
            }

            int interval = mBossMode ? 20 + (10 - Challenge ) * 3 : 11 - Challenge;

            game = new BattleGame(playerCombatant, EnemyCombatant, 2, Operator == "x" ? 9 : multFactors.FirstOrDefault(), interval, multFactors, new List<string>() { Operator });
            game.EnemyPoll += EnemyAct;
            game.EnemyTimerAttack += EnemyTimerAttack;
            game.Victory += Victory;
            game.Defeat += Defeat;

            RightCombatantViewModel.Refresh();
            LeftCombatantViewModel.Refresh();
            CurrentProblem = game.GetProblem();

            QuestionViewModel = new TriangleFactViewModel(CurrentProblem, VoiceMode);

            SetGameActive(true);
            
        }

        private  Combatant GetCombatant(int MultFactor, double difficultyFactor, bool bossMode, Combatant playerCombatant)
        {
            Combatant comb;

            if (bossMode)
            {
                comb = new Combatant(string.Concat(MultFactor, "-Boss"), 100, 0, 9999, 9999, "Boss.png");
                comb.EmptyBag = () =>
                    {
                        comb.PhysicalDefense = 0;
                        playerCombatant.PhysicalAttack = 9999;
                        playerCombatant.Attack(comb);
                    };
                   
            }
            else
            {
                switch (MultFactor)
                {
                    case 3:
                        comb = new Combatant("Three-Man", 80, 0, (int)(7 * difficultyFactor), 3, "ThreeMan.png");
                        break;
                    case 4:
                        comb = new Combatant("Four-Man", 80, 0, (int)(7 * difficultyFactor), 3, "FourMan.png");
                        break;
                    default:
                        comb = new Combatant(string.Concat(MultFactor, "-Guy"), 80, 0, (int)(7 * difficultyFactor), 3, "DefaultGuy.png");
                        break;
                }
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
            Challenge = 7;
            MultFactor = "5";
            MusicVolume = 0;
            Operator = "*";
            GameStatusText = "Ready To Start...";
            resources = new ResponseResources(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Images\\"), new List<string> { "correct", "wrong", "battle", "bang", "hit", "magic" });

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
            QuestionViewModel = new TriangleFactViewModel(CurrentProblem, VoiceMode);
            
            NotifyPropertyChanged(string.Empty);
        }

        public void UseMagic()
        {
            if(mBossMode)
            {
                return;
            }

            if (LeftCombatantViewModel.CombatantData.CurrentMana >= 20)
            {
                LeftCombatantViewModel.CombatantData.CurrentMana -= 20;
                QuestionViewModel.ShowAnswer();
                resources.GetResponse("magic").PlaySound();
                LeftCombatantViewModel.Refresh();
            }
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
            game.ActiveGame = false;
            MusicVolume = 0;
            NotifyPropertyChanged(string.Empty);
        }

        public void Defeat(object sender, EventArgs e)
        {
            GameStatusText = "Try again next time.";
            game.ActiveGame = false;
            MusicVolume = 0;
            NotifyPropertyChanged(string.Empty);
        }

        bool mBossMode;
        public bool BossMode
        {
            get { return mBossMode; }
            set
            {
                mBossMode = value;
                NotifyPropertyChanged("BossMode");
            }

        }

        bool mVoiceMode;
        public bool VoiceMode
        {
            get { return mVoiceMode; }
            set
            {
                mVoiceMode = value;
                NotifyPropertyChanged("VoiceMode");
            }
        }

        string mOperator;
        public string Operator
        {
            get { return mOperator; }
            set
            {
                mOperator = value;
                NotifyPropertyChanged("Operator");
            }
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
                NotifyPropertyChanged("Challenge");

            }
        }
    }
}
