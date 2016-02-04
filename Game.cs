using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningGame.Core
{
    public abstract class Game
    {
        internal ProblemGenerator Generator { get; set; }
        public List<Problem> Problems { get; set; }
        public int Score { get; set; }
        public Game(int lowerBound, int upperBound, List<string> operators, int problemCount)
        {

        }

        public bool AttemptProblem(Problem problem, int answer)
        {
            bool correct = problem.IsSolution(answer);
            if(correct)
            {
                Score++;
            }
            else
            {
                // nothingl
            }

            return correct;
        }
    }
}
