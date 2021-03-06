﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningGame.Core
{
    public class ProblemGenerator
    {
        public int LowerBound;
        public int UpperBound;
        List<string> Operators;
        Random rnd = new Random();

        public ProblemGenerator(int lowerBound, int upperBound, List<string> operators)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Operators = operators;
        }

        public ProblemGenerator(int lowerBound, int upperBound) : this(lowerBound, upperBound, new List<string>() { "+", "-" })
        {

        }

        public Problem GenerateProblem()
        {

            int opTypeIndex = rnd.Next(0, Operators.Count());
            string operatorType = Operators[opTypeIndex];

            Problem returnProb;
            int a = rnd.Next(LowerBound, UpperBound+1);
            int b = rnd.Next(LowerBound, UpperBound+1);

            switch (operatorType)
            {
                case "+":
                    returnProb = new AdditionProblem(a, b);
                    break;
                case "-":
                    returnProb = new SubtractionProblem(Math.Max(a, b), Math.Min(a, b));
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return returnProb;
        }
    }
}
