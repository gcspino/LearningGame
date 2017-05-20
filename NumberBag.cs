using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningGame.Core
{


    class NumberBag
    {
        private List<int> mBag;
        private List<int> mOriginalBag;

        Random rnd = new Random();

        public NumberBag(List<int> numbers)
        {
            mOriginalBag = numbers;
            mBag = numbers.ToList();
        }

        public NumberBag(int lowerBound, int upperBound)
        {
            List<int> origBag = new List<int>();
            for(int i = lowerBound; i <= upperBound; i++)
            {
                origBag.Add(i);
            }
            mOriginalBag = origBag;
            mBag = mOriginalBag.ToList();
        }

        public bool HasNumbers()
        {
            return mBag.Any();
        }

        public void RefreshBag()
        {
            mBag = mOriginalBag.ToList();
        }

        public int DrawNumber(bool autoRefresh)
        {
            if(HasNumbers())
            {
                int drawIndex = rnd.Next(0, mBag.Count());

                int drawNumber = mBag[drawIndex];

                mBag.Remove(drawNumber);

                return drawNumber;
            }
            else if(autoRefresh)
            {
                RefreshBag();
                return DrawNumber(false);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
