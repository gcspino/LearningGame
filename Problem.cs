namespace LearningGame.Core
{
    public abstract class Problem
    {
        public abstract int Solution();
        public bool IsSolution(int answer)
        {
            return answer == Solution();
        }

        public abstract string Operator { get; }

        public int A { get; set; }
        public int B { get; set; }

        public Problem()
        {

        }

        public string AnswerText()
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
