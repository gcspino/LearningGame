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
        public Action EmptyBagAction = null;
        private bool mAutoRefreshBag = true;

        Random rnd = new Random();

        public NumberBag(List<int> numbers, bool autoRefreshBag, Action emptyBagAction)
        {
            mOriginalBag = numbers;
            mBag = numbers.ToList();
            mAutoRefreshBag = autoRefreshBag;
            EmptyBagAction = emptyBagAction;   
        }

        public NumberBag(int lowerBound, int upperBound, bool autoRefreshBag, Action emptyBagAction)
        {
            List<int> origBag = new List<int>();
            for(int i = lowerBound; i <= upperBound; i++)
            {
                origBag.Add(i);
            }
            mOriginalBag = origBag;
            mBag = mOriginalBag.ToList();
            mAutoRefreshBag = autoRefreshBag;
            EmptyBagAction = emptyBagAction;
        }

        public bool HasNumbers()
        {
            return mBag.Any();
        }

        public void RefreshBag()
        {
            mBag = mOriginalBag.ToList();
        }

        public int DrawNumber()
        {
            if(HasNumbers())
            {
                int drawIndex = rnd.Next(0, mBag.Count());

                int drawNumber = mBag[drawIndex];

                mBag.Remove(drawNumber);

                return drawNumber;
            }
            else if(mAutoRefreshBag)
            {
                RefreshBag();
                return DrawNumber();
            }
            else
            {
                EmptyBagAction();
                return 0;
            }
        }

    }
}
