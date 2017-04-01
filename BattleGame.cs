using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LearningGame.Core
{
    public class BattleGame : Game
    {
        public Combatant Player { get; set; }
        public Combatant Opponent { get; set; }
        public bool ActiveGame { get; set; }

        DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        protected virtual void OnVictory(EventArgs e)
        {
            EventHandler handler = Victory;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler Victory;

        protected virtual void OnDefeat(EventArgs e)
        {
            EventHandler handler = Defeat;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler Defeat;

        protected virtual void OnEnemyPoll(EventArgs e)
        {
            EventHandler handler = EnemyPoll;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler EnemyPoll;

        protected virtual void OnEnemyTimerAttack(EventArgs e)
        {
            EventHandler handler = EnemyTimerAttack;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler EnemyTimerAttack;

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if(ActiveGame)
            {
                Opponent.Attack(Player);
                OnEnemyTimerAttack(e);
                OnEnemyPoll(e);
            }
        }

        public BattleGame(Combatant player, Combatant opponent, int lowerBound, int upperBound, int secondsBetweenActions, 
            List<int> factors,
            List<string> operators)
            : base(lowerBound, upperBound, operators, 1)
        {
            Generator = new ProblemGenerator(lowerBound, upperBound, operators, factors);
            Problems = new List<Problem>();

            Player = player;
            Player.Defeat += PlayerDefeat;
            Opponent = opponent;
            Opponent.Defeat += PlayerVictory;

            Problem problem = Generator.GenerateProblem();
            Problems.Add(problem);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, secondsBetweenActions);
            dispatcherTimer.Start();

            ActiveGame = false;
        }

        public Problem GetProblem()
        {
            return Generator.GenerateProblem();
        }

        public void PlayerDefeat(object sender, EventArgs e)
        {
            ActiveGame = false;
            OnDefeat(e);
        }

        public void PlayerVictory(object sender, EventArgs e)
        {
            ActiveGame = false;
            OnVictory(e);
        }


    }
}
