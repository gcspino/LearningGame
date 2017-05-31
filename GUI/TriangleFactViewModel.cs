using LearningGame.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace LearningGame.GUI
{
    public class TriangleFactViewModel : INotifyPropertyChanged
    {
        Problem mProblem;
        Random rnd = new Random();
        SpeechSynthesizer synth;

        public event PropertyChangedEventHandler PropertyChanged;
        private bool VoiceMode;

        private void NotifyPropertyChanged(string p)
        {
            if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public TriangleFactViewModel(Problem problem, bool voiceMode)
        {
            VoiceMode = voiceMode;

            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();

            ProblemData = problem;
        }

        public void ShowAnswer()
        {
            mProblemTop = ProblemData.Solution().ToString();
            NotifyPropertyChanged("ProblemTop");
        }

        public Problem ProblemData
        {
            get { return mProblem; }
            set
            {
                mProblem = value;
                
                switch(ProblemData.Operator)
                {
                    case "|":
                        mProblemTop = "?";
                        mProblemLeft = "1-Y";
                        mProblemRight = "0-N";
                        mMiddleValue = "Right?";
                        break;
                    case "*":
                        mProblemTop = "?";
                        mProblemLeft = ProblemData.A.ToString();
                        mProblemRight = ProblemData.B.ToString();
                        mMiddleValue = "x";
                        if(VoiceMode)
                        {
                            synth.SpeakAsync(string.Format("{0} times {1}", ProblemData.A.ToString(), ProblemData.B.ToString()));
                        }
                        break;
                    case "/":
                        mProblemTop = ProblemData.A.ToString();
                        if (rnd.Next(0, 2) > 0)
                        {
                            mProblemLeft = "?";
                            mProblemRight = ProblemData.B.ToString();
                           
                        }
                        else
                        {
                            mProblemLeft = ProblemData.B.ToString(); 
                            mProblemRight = "?";
                        }
                        mMiddleValue = "* /";
                        break;
                }
                NotifyPropertyChanged(string.Empty);
            }
        }

        private string mProblemTop;
        public string ProblemTop
        {
            get
            {
                return mProblemTop;
            }
        }

        private string mProblemLeft;
        public string ProblemLeft
        {
            get
            {
                return mProblemLeft;
            }
        }

        private string mProblemRight;
        public string ProblemRight
        {
            get
            {
                return mProblemRight;
            }
        }

        private string mMiddleValue;
        public string MiddleValue
        {
            get
            {
                return mMiddleValue;
            }
        }
    }
}