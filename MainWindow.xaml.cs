using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int playerTurn = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = new Ellipse();//Avoir de pions à la place
            ellipse.Height = 40;
            ellipse.Width = 40;
            ellipse.Stroke = Brushes.Black;
            if (playerTurn % 2 == 0)
            {
                ellipse.Fill = Brushes.Black;//Alterner les tours
            }
            else
            {
                ellipse.Fill = Brushes.White;//Alterner les tours
            }
            
            othelloBoard.Children.Add(ellipse);
            playerTurn++;

            var element = (UIElement)e.Source;
            int c = Grid.GetColumn(element);
            int r = Grid.GetRow(element);
            Grid.SetColumn(ellipse,c);
            Grid.SetRow(ellipse, r);
        }
    }
}
