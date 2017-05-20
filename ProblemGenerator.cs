using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningGame.Core
{
    internal class ProblemGenerator
    {
        public int LowerBound;
        public int UpperBound;
        List<string> Operators;
        List<int> OtherFactors;
        Random rnd = new Random();
        Dictionary<string,NumberBag> numberBags;
        public ProblemGenerator(int lowerBound, int upperBound, List<string> operators, List<int> otherFactors)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Operators = operators;
            OtherFactors = otherFactors;

            numberBags = operators.ToDictionary(c => c, d => new NumberBag(lowerBound, upperBound));
        }
        public Problem GenerateProblem()
        {

            int opTypeIndex = rnd.Next(0, Operators.Count());
            string operatorType = Operators[opTypeIndex];

            Problem returnProb;
            int a = rnd.Next(LowerBound, UpperBound+1);
            int b = rnd.Next(LowerBound, UpperBound+1);
            int factor = OtherFactors[rnd.Next(0, OtherFactors.Count())];
            NumberBag operatorBag = numberBags[operatorType];

            switch (operatorType)
            {
                case "|":
                    returnProb = new CorrectIncorrectProblem(1, 1);
                    break;
                case "+":
                    returnProb = new AdditionProblem(a, b);
                    break;
                case "-":
                    returnProb = new SubtractionProblem(Math.Max(a, b), Math.Min(a, b));
                    break;
                case "*":
                    if(rnd.Next(0,2) < 1)
                    {
                        a = factor;
                    }
                    else
                    {
                        b = factor;
                    }
                    returnProb = new MultiplicationProblem(a, b);
                    break;
                case "x":

                    int pulledNumber = operatorBag.DrawNumber(true);

                    if (rnd.Next(0, 2) < 1)
                    {
                        a = pulledNumber;
                        b = factor;
                    }
                    else
                    {
                        a = factor;
                        b = pulledNumber;
                    }
                    returnProb = new MultiplicationProblem(a, b);
                    break;
                case "/":
                    if (rnd.Next(0, 2) < 1)
                    {
                        a = factor;
                    }
                    else
                    {
                        b = factor;
                    }
                    int answer = a * b;
                    int secondNumber;
                    if(rnd.Next(0, 2) < 1)
                    {
                        secondNumber = factor;
                    }
                    else
                    {
                        secondNumber = answer / factor;
                    }
                    returnProb = new DivisionProblem(answer, secondNumber);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return returnProb;
        }
    }
}
