using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningGame.Core
{
    public abstract class Problem
    {
        public abstract int Solution();
        public bool IsSolution(int answer)
        {
            return answer == Solution();
        }

        public bool IsAnswer(string answer)
        {
            return answer.ToLower() == AnswerText().ToLower();
        }

        public abstract string Operator { get; }

        public int A { get; set; }
        public int B { get; set; }

        public string Prompt { get; set; }
        public string Answer { get; set; }
        public bool HideTextPrompt { get; set; }    

        public Problem()
        {

        }

        public virtual string AnswerText()
        {
            return string.Concat(A, Operator, B, "=", Solution());
        }

        public Problem(int a, int b)
        {
            A = a;
            B = b;
        }

    }
}
