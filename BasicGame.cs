using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningGame.Core
{
    public class BasicGame : Game
    {

        public BasicGame(int lowerBound, int upperBound, List<string> operators, int problemCount) 
            : base(lowerBound, upperBound,operators, problemCount)
        {
            Generator = new ProblemGenerator(lowerBound, upperBound, operators, null, true ,()=> { });
            Problems = new List<Problem>();

            for (int i = 0; i < problemCount; i++)
            {
                Problem problem = Generator.GenerateProblem();
                Problems.Add(problem);
            }
        }

        
    }
}
