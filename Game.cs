using System.Collections.Generic;

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
