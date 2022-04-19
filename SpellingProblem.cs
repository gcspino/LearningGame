using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningGame.Core
{
    public class SpellingProblem : Problem
    {
        public SpellingProblem(string prompt, string answer)
        {
            this.Prompt = prompt;
            this.Answer = answer;
        }
        public SpellingProblem(int a, int b) : base(a, b)
        {
        }

        public override string Operator
        {
            get
            {
                return "Spell";
            }
        }

        public override string AnswerText()
        {
            return Answer.ToLower();
        }

        public override int Solution()
        {
            return 0;
        }

    }
}
