﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningGame.Core
{
    public class MultiplicationProblem : Problem
    {
        public MultiplicationProblem(int a, int b) : base(a, b)
        {
        }

        public override string Operator
        {
            get
            {
                return "*";
            }
        }

        public override int Solution()
        {
            return this.A * B;
        }


    }
}