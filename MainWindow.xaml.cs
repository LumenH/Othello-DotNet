﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
using Othello.Annotations;

namespace Othello
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int playerTurn = 0;

        private LogicBoard logicBoard;

        private readonly SolidColorBrush playableColor = new SolidColorBrush(Color.FromRgb(0, 111, 111));

        private readonly SolidColorBrush regularColor = new SolidColorBrush(Colors.Green);

        public int BlackScore => logicBoard.getBlackScore();

        public int WhiteScore => logicBoard.getWhiteScore();

        public string BlackTimeLeft => $"{blackPlayer.MinutesLeft} : {blackPlayer.SecondsLeft}";

        public string WhiteTimeLeft => $"{whitePlayer.MinutesLeft} : {whitePlayer.SecondsLeft}";

        public string CurrentTurn => $"Player {playerTurn} turn BITCH";

        private List<Ellipse> pawns = new List<Ellipse>();
        
        public Player whitePlayer = new Player("WhitePlayer", Pawn.Colors.White);

        public Player blackPlayer = new Player("BlackPlayer", Pawn.Colors.Black);

        DispatcherTimer mainTimer = new DispatcherTimer();
        private System.IO.Stream stream;
        public Save save;

        public MainWindow()
        {
            InitializeComponent();
            
            logicBoard = new LogicBoard();
            logicBoard.fillFakeBoard();
            loadFromLogicBoard();
            DataContext = this;
            mainTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};

            mainTimer.Tick += (sender, args) =>
            {
                whitePlayer.tick();
                blackPlayer.tick();

                NotifyPropertyChanged("WhiteTimeLeft");
                NotifyPropertyChanged("BlackTimeLeft");
            };

            mainTimer.Start();

            whitePlayer.start();
        }
        

        private void mouseDown(object sender, MouseButtonEventArgs e)
        {

            var element = (UIElement)e.Source;
            var c = Grid.GetColumn(element);
            var r = Grid.GetRow(element);

            var playable = logicBoard.isPlayable(c, r, playerTurn % 2 != 0);

            if (playable)
            {
                logicBoard.playMove(c, r, playerTurn%2 != 0);

                loadFromLogicBoard();

                if (playerTurn%2 == 0)
                {
                    whitePlayer.stop();
                    blackPlayer.start();
                }
                else
                {
                    blackPlayer.stop();
                    whitePlayer.start();
                }


                if (CanPlayerPlay())
                    playerTurn++;
                else
                    Console.WriteLine("Humm seems that you can't play !");

                NotifyPropertyChanged("BlackScore");
                NotifyPropertyChanged("WhiteScore");
                NotifyPropertyChanged("CurrrentTurn");



            }

            
        }

        private void loadFromLogicBoard()
        {
            pawns.ForEach(p => othelloBoard.Children.Remove(p));
            pawns.Clear();

            for (var i = 0; i < LogicBoard.HEIGHT; i++)
            {
                for (var j = 0; j < LogicBoard.WIDTH; j++)
                {
                    var value = logicBoard.Board[i, j];

                    if (value != null)
                    {
                        var ellipse = new Ellipse
                        {
                            Height = 40,
                            Width = 40,
                            Stroke = Brushes.Black,
                            Fill = value.Color == Pawn.Colors.White ? Brushes.White : Brushes.Black
                        };
                        pawns.Add(ellipse);

                        Grid.SetColumn(ellipse, j);
                        Grid.SetRow(ellipse, i);
                    }
                }
            }
            pawns.ForEach(p => othelloBoard.Children.Add(p));
        }

        private void menuExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnBoardMouseMove(object sender, MouseEventArgs e)
        {
            var source = e.Source as Rectangle;

            if (source == null) return;

            var rectHover = source;
            var x = Grid.GetColumn(rectHover);
            var y = Grid.GetRow(rectHover);

            var playable = logicBoard.isPlayable(x, y, playerTurn % 2 != 0);

            othelloBoard.Children.Cast<UIElement>()
                .ToList()
                .Where(c => c is Rectangle)
                .Cast<Rectangle>()
                .ToList()
                .ForEach(c => c.Fill = c.Equals(rectHover) && playable ? playableColor : regularColor);
            
        }
        

        public bool CanPlayerPlay()
        {
            return othelloBoard.Children.Cast<UIElement>()
                .ToList()
                .Where(c => c is Rectangle)
                .Cast<Rectangle>()
                .ToList()
                .Where(c => logicBoard.Board[Grid.GetRow(c), Grid.GetColumn(c)] == null)
                .Where(c => logicBoard.isPlayable(Grid.GetColumn(c), Grid.GetRow(c), playerTurn%2 != 0)).Count() > 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void saveClick(object sender, RoutedEventArgs e)
        {

            if(playerTurn%2 == 0)
            {
                //black is current player
                save = new Save(logicBoard, blackPlayer, whitePlayer, playerTurn);
            }
            else
            {
                save = new Save(logicBoard, whitePlayer, blackPlayer, playerTurn);
            }
            

            stream = System.IO.File.Open("save.xml", System.IO.FileMode.Create);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            bformatter.Serialize(stream, save);
            stream.Close();

            MessageBox.Show("Sauvegarde réussie");
        }

        private void loadClick(object sender, RoutedEventArgs e)
        {
            save = null;

            stream = System.IO.File.Open("save.xml", System.IO.FileMode.Open);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

             save = (Save)bformatter.Deserialize(stream);
            stream.Close();

            logicBoard = save.Board;
            playerTurn = save.turn;

            if(playerTurn%2 == 0)
            {
                blackPlayer = save.CurrentPlayer;
                whitePlayer = save.Player2;
            }
            else
            {
                whitePlayer = save.CurrentPlayer;
                blackPlayer = save.Player2;
            }

            MessageBox.Show("Load réussi");

            loadFromLogicBoard();
        }
    }
}
