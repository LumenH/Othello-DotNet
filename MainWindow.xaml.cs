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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Height = 40;
            ellipse.Width = 40;
            ellipse.Stroke = Brushes.Black;
            ellipse.Fill = Brushes.Black;
            othelloBoard.Children.Add(ellipse);
            Grid.SetColumn(ellipse,Convert.ToInt32(e.GetPosition(othelloBoard).X));//Ne fais pas du tout ce que je veux
            Grid.SetRow(ellipse, Convert.ToInt32(e.GetPosition(othelloBoard).Y));
        }
    }
}
