using System.Collections.Generic;

namespace LearningGame.Core
{
    public class BasicGame : Game
    {

        public BasicGame(int lowerBound, int upperBound, List<string> operators, int problemCount) 
            : base(lowerBound, upperBound,operators, problemCount)
        {
            Generator = new ProblemGenerator(lowerBound, upperBound, operators);
            Problems = new List<Problem>();

            for (int i = 0; i < problemCount; i++)
            {
                Problem problem = Generator.GenerateProblem();
                Problems.Add(problem);
            }
        }

        
    }
}
