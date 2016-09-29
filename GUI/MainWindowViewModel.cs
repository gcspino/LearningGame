using LearningGame.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;

namespace LearningGame.GUI
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private BasicGame game;
        private int problemIndex = 0;
        private int attempts = 0;

        private ResponseResources resources;
        private ResponseUIPair BackgroundResource;
        

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
            resources = new ResponseResources(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Images\\"), new List<string> { "correct", "wrong", "battle" });
            BackgroundResource = resources.GetResponse("battle");
        }

        public void AnswerCurrentProblem(int answer)
        {
            Feedback = "";
            if (game.AttemptProblem(CurrentProblem, answer))
            {
                // correct
                SetFeedback("correct");
                problemIndex++;
                attempts = 0;
            }
            else
            {
                // incorrect
                attempts++;
                if (attempts > 1)
                {
                    SetFeedback("wrong");
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

        private void SetFeedback(string category)
        {
            ResponseUIPair responsePair = resources.GetResponse(category);
            ResponseImage = responsePair.Image;
            responsePair.PlaySound();
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

        private BitmapImage mResponseImage;
        public BitmapImage ResponseImage
        {
            get
            {
                return mResponseImage;
            }

            set
            {
                mResponseImage = value;
                NotifyPropertyChanged("ResponseImage");
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
