using LearningGame.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningGame.GUI
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private BasicGame game;
        private int problemIndex = 0;
        private int attempts = 0;

        public Problem CurrentProblem { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string p)
        {
            if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public MainWindowViewModel()
        {
            game = new BasicGame(0, 10, new List<string>() { "+", "-" }, 15);
            CurrentProblem = game.Problems[0];
            NotifyPropertyChanged(string.Empty);
        }

        public void AnswerCurrentProblem(int answer)
        {
            Feedback = "";
            if (game.AttemptProblem(CurrentProblem, answer))
            {
                // correct
                problemIndex++;
                attempts = 0;
            }
            else
            {
                // incorrect
                attempts++;
                if (attempts > 1)
                {
                    Feedback = CurrentProblem.AnswerText();
                    problemIndex++;
                    attempts = 0;
                }
                else
                {
                    Feedback = "Try again!";
                }
            }

            if (problemIndex >= game.Problems.Count())
            {
                CurrentProblem = null;
                Feedback = string.Concat("Complete: ", Score, "/", game.Problems.Count());
            }
            else
            {
                CurrentProblem = game.Problems[problemIndex];
            }
            NotifyPropertyChanged(string.Empty);
        }

        private string mFeedback;

        public string Feedback
        {
            get
            {
                return mFeedback;
            }

            set
            {
                mFeedback = value;
                NotifyPropertyChanged("Feedback");
            }
        }

        public string Score
        {
            get
            {
                return string.Concat("Score: ", game.Score);
            }
        }
    }
}
