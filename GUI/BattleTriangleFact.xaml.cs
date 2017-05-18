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
    public partial class BattleTriangleFact : Window
    {

        BattleTriangleFactViewModel vm;

        public BattleTriangleFact()
        {
            InitializeComponent();
            vm = new BattleTriangleFactViewModel();
            DataContext = vm;
            Answer.Focus();
        }

        private void Answer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (vm.CurrentProblem == null)
                {
                    return;
                }
                int answer;
                if (int.TryParse(Answer.Text, out answer))
                {
                    vm.AnswerCurrentProblem(answer);
                }
                Answer.Text = "";
            }
            else if(e.Key == Key.M)
            {
                e.Handled = true;
                vm.UseMagic();
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroudMedia.Position = TimeSpan.FromSeconds(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.SetupBattlers();
            vm.SetGameActive(true);
            Answer.Focus();
        }
    }
}
