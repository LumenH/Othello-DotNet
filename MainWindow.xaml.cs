﻿using System;
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
        private int nbBlackPawn = 2;
        private int nbWithPawn = 2;
        public MainWindow()
        {
            InitializeComponent();
            /*
            LogicBoard logicBoard = new LogicBoard();
            logicBoard.fillFakeBoard();
            logicBoard.isPlayable(1, 2, true);
            */
            nbPawnPlayer2.Content = Convert.ToString(nbBlackPawn);
            nbPawnPlayer1.Content = Convert.ToString(nbWithPawn);
        }

        private void mouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = new Ellipse();//Avoir de pions à la place
            ellipse.Height = 40;
            ellipse.Width = 40;
            ellipse.Stroke = Brushes.Black;
            if (playerTurn % 2 == 0)
            {
                //Les noirs sont le joueur 2
                ellipse.Fill = Brushes.Black;
                turn.Content = "Player 1 turn";
                nbBlackPawn++;
                nbPawnPlayer2.Content = Convert.ToString(nbBlackPawn);
            }
            else
            {
                //Les blancs sont le joueur 1
                ellipse.Fill = Brushes.White;
                turn.Content = "Player 2 turn";
                nbWithPawn++;
                nbPawnPlayer1.Content = Convert.ToString(nbWithPawn);
            }
            
            othelloBoard.Children.Add(ellipse);
            playerTurn++;

            var element = (UIElement)e.Source;
            int c = Grid.GetColumn(element);
            int r = Grid.GetRow(element);
            Grid.SetColumn(ellipse,c);
            Grid.SetRow(ellipse, r);
        }

        private void loadWindow(object sender, RoutedEventArgs e)
        {
            //Première pièce blanche
            Ellipse ellipseW = new Ellipse();
            ellipseW.Height = 40;
            ellipseW.Width = 40;
            ellipseW.Stroke = Brushes.Black;
            ellipseW.Fill = Brushes.White;

            othelloBoard.Children.Add(ellipseW);

            Grid.SetColumn(ellipseW, 3);
            Grid.SetRow(ellipseW, 3);

            //Deuxième pièce blanche
            Ellipse ellipseW1 = new Ellipse();
            ellipseW1.Height = 40;
            ellipseW1.Width = 40;
            ellipseW1.Stroke = Brushes.Black;
            ellipseW1.Fill = Brushes.White;

            othelloBoard.Children.Add(ellipseW1);

            Grid.SetColumn(ellipseW1, 4);
            Grid.SetRow(ellipseW1, 4);

            //Première pièce noire
            Ellipse ellipseB = new Ellipse();
            ellipseB.Height = 40;
            ellipseB.Width = 40;
            ellipseB.Stroke = Brushes.Black;
            ellipseB.Fill = Brushes.Black;

            othelloBoard.Children.Add(ellipseB);

            Grid.SetColumn(ellipseB, 3);
            Grid.SetRow(ellipseB, 4);

            //Deuxième pièce noire
            Ellipse ellipseB1 = new Ellipse();
            ellipseB1.Height = 40;
            ellipseB1.Width = 40;
            ellipseB1.Stroke = Brushes.Black;
            ellipseB1.Fill = Brushes.Black;

            othelloBoard.Children.Add(ellipseB1);

            Grid.SetColumn(ellipseB1, 4);
            Grid.SetRow(ellipseB1, 3);
        }

        private void menuExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
>>>>>>> b486d9ee1a6c9038f324c1b590d177ecd7bc51c4
        }
    }
}
