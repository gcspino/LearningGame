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



        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // code goes here
        }

        public BattleGame(Combatant player, Combatant opponent, int lowerBound, int upperBound, List<string> operators)
            : base(lowerBound, upperBound, operators, 1)
        {
            Generator = new ProblemGenerator(lowerBound, upperBound, operators, new List<int>() { 2, 5, 10 });
            Problems = new List<Problem>();

            Player = player;
            Opponent = opponent;

            Problem problem = Generator.GenerateProblem();
            Problems.Add(problem);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();

            ActiveGame = false;
        }

        public Problem GetProblem()
        {
            return Generator.GenerateProblem();
        }

    }
}
