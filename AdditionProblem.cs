using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearningGame.Core
{
    public class AdditionProblem : Problem
    {
        public AdditionProblem(int a, int b) : base(a, b)
        {
        }

        public override string Operator
        {
            get
            {
                return "+";
            }
        }

        public override int Solution()
        {
            return this.A + B;
        }


    }
}
