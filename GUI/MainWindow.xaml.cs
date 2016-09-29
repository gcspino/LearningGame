using LearningGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Media;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LearningGame.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MainWindowViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewModel();
            DataContext = vm;
            Answer.Focus();
        }

        private void Answer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(vm.CurrentProblem == null)
                {
                    vm = new MainWindowViewModel();
                    DataContext = vm;
                }
                int answer;
                if (int.TryParse(Answer.Text, out answer))
                {
                    vm.AnswerCurrentProblem(answer);
                }
                Answer.Text = "";
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroudMedia.Position = TimeSpan.FromSeconds(0);
        }
    }
}
