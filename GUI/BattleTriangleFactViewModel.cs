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
        public PlayerState State { get; set; }
        public double MusicVolume { get; set; }
        public string GameStatusText { get; set; }
        public double GoldPayment { get; set; }

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
            if (int.TryParse(multFactors, out singleFactor))
            {
                return new List<int>() { singleFactor };
            }
            else
            {
                string[] splitFactors = multFactors.Split(new string[] { ",", ";", " " }, StringSplitOptions.RemoveEmptyEntries);
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

                    if (!factors.Any())
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
                if (Operator == "+" || Operator == "-")
                {
                    EnemyCombatant.CurrentHP = 15;
                    RightCombatantViewModel.Refresh();
                }
                playerCombatant.CurrentMana = 0;
            }

            int interval = mBossMode ? 1 : 11 - Challenge;
            game = new BattleGame(playerCombatant, EnemyCombatant, 2, Operator == "x" || Operator == "*" ? 9 : multFactors.FirstOrDefault(), interval, multFactors, new List<string>() { Operator });
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

        private Combatant GetCombatant(int MultFactor, double difficultyFactor, bool bossMode, Combatant playerCombatant)
        {
            Combatant comb;

            if (bossMode)
            {
                comb = new Combatant(string.Concat(MultFactor, "-Boss"), 100, 100, 0, 9999, "Boss.png");
                comb.CurrentMana = comb.MaxMana - (20 + (10 - Challenge) * 3);
                comb.Pulse += () =>
                {
                    if (comb.CurrentMana == comb.MaxMana)
                    {
                        comb.PhysicalAttack = 9999;
                    }
                    else
                    {
                        comb.CurrentMana += 1;
                    }

                };
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
                    case 5:
                        comb = new Combatant("Five-Woman", 80, 0, (int)(7 * difficultyFactor), 3, "FiveWoman.png");
                        break;
                    case 7:
                        comb = new Combatant("Seven-Woman", 80, 0, (int)(7 * difficultyFactor), 3, "SevenWoman.png");
                        break;
                    case 6:
                        comb = new Combatant("Six-Woman", 80, 0, (int)(7 * difficultyFactor), 3, "SixWoman.png");
                        break;
                    case 8:
                        comb = new Combatant("Eight-Monster", 80, 0, (int)(7 * difficultyFactor), 3, "EightMonster.png");
                        break;
                    case 9:
                        comb = new Combatant("Nine-Woman", 80, 0, (int)(7 * difficultyFactor), 3, "NineWoman.png");
                        break;
                    default:
                        comb = new Combatant(string.Concat(MultFactor, "-Guy"), 80, 0, (int)(7 * difficultyFactor), 3, "DefaultGuy.png");
                        break;
                }
            }
            return comb;
        }

        public void SetGameActive(bool gameIsActive)
        {
            game.ActiveGame = gameIsActive;
            GameStatusText = "Battle";

            if (gameIsActive)
            {
                MusicVolume = 0.25;
            }
            NotifyPropertyChanged(string.Empty);
        }

        public void PayGold(double paymentAmountCode)
        {
            int paymentAmount = (int)paymentAmountCode;

            int enteredCode = (int)((paymentAmountCode - paymentAmount) * 1000000);
            int masterCode = DateTime.Now.Month * 10 + DateTime.Now.Day * 1000 + (DateTime.Now.Month + DateTime.Now.Day) % 10;
            if (Math.Abs(masterCode - enteredCode) <= 1
                && paymentAmount <= State.Gold
                && paymentAmount > 0)
            {
                State.Gold -= paymentAmount;
                NotifyPropertyChanged(string.Empty);
                PlayerStateHelper.WriteState(State);
            }

        }
        public BattleTriangleFactViewModel()
        {
            Challenge = 7;
            MultFactor = "5";
            MusicVolume = 0;
            Operator = "x";
            GameStatusText = "Ready To Start...";
            resources = new ResponseResources(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Images\\"), new List<string> { "correct", "wrong", "battle", "bang", "hit", "magic" });

            BackgroundResource = resources.GetResponse("battle");
        }

        public void AnswerCurrentProblem(int answer)
        {
            if (game.Opponent.CurrentHP == 0)
            {
                return;
            }

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
            if (mBossMode)
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
            if (RightCombatantViewModel.CombatantData.PhysicalAttack >= 1)
            {
                resources.GetResponse("bang").PlaySound();
            }
            RightCombatantViewModel.CombatantData.Pulse?.Invoke();
        }


        public void Victory(object sender, EventArgs e)
        {
            int goldReward = PlayerStateHelper.GetReward(this.GetMultFactors(this.MultFactor),
                this.Operator, this.Challenge, LeftCombatantViewModel.CombatantData,
                RightCombatantViewModel.CombatantData, this.BossMode);
            GameStatusText = string.Format("Winner!!! You earn {0} gold!", goldReward);
            game.ActiveGame = false;
            MusicVolume = 0;
            State.Gold += goldReward;
            NotifyPropertyChanged(string.Empty);
            PlayerStateHelper.WriteState(State);
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

        public string PlayerGoldText
        {
            get
            {
                return string.Format("Gold: {0}", State.Gold);
            }
        }
    }
}
