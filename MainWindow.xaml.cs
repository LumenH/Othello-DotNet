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
using System.IO;
using System.Windows.Forms;

namespace Othello
{
    /// <summary>
    /// Interaction de la GUI
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

        public string CurrentTurn => $"Player {(playerTurn%2 == 0 ? "black" : "white")} turn";




        private List<Ellipse> pawns = new List<Ellipse>();
        
        public Player whitePlayer = new Player("WhitePlayer", Pawn.Colors.White);

        public Player blackPlayer = new Player("BlackPlayer", Pawn.Colors.Black);

        DispatcherTimer mainTimer = new DispatcherTimer();

        private System.IO.Stream stream;
        public Save save;

        public MainWindow()
        {
            InitializeComponent();
            
            // Chargement du board
            logicBoard = new LogicBoard();
            logicBoard.fillBoard();
            loadFromLogicBoard();
            DataContext = this;
            mainTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};

            //Timer principale pour les 2 horloges
            mainTimer.Tick += (sender, args) =>
            {
                whitePlayer.tick();
                blackPlayer.tick();

                NotifyPropertyChanged("WhiteTimeLeft");
                NotifyPropertyChanged("BlackTimeLeft");

               if(IsGameFinished())
                {
                    NotifyPropertyChanged("BlackScore");
                    NotifyPropertyChanged("WhiteScore");
                    NotifyPropertyChanged("CurrentTurn");
                    EndGame();
                }
            };

            mainTimer.Start();
        }
        
        /// <summary>
        /// Event appeler lors du click de la souris sur le board.
        /// Cette méthode check si le coup est jouable et l'applique.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    whitePlayer.start();
                    blackPlayer.stop();
                }
                else
                {
                    blackPlayer.start();
                    whitePlayer.stop();
                }

                playerTurn++;

                if (!CanPlayerPlay(playerTurn) && !IsGameFinished())
                {
                    playerTurn++;
                    System.Windows.MessageBox.Show("You can not play this turn");
                }

                Console.WriteLine($" player turn is {playerTurn}");

                NotifyPropertyChanged("BlackScore");
                NotifyPropertyChanged("WhiteScore");
                NotifyPropertyChanged("CurrentTurn");

            }
        }

        /// <summary>
        /// Met à jours le board GUI avec le board Logic
        /// </summary>
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
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Check lors du survol de la souris si le coup est jouable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBoardMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
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
        
        /// <summary>
        /// Check si pour le joueur spécifié il reste un coup jouable
        /// </summary>
        /// <param name="playerTurn"></param>
        /// <returns></returns>
        public bool CanPlayerPlay(int playerTurn)
        {
            return othelloBoard.Children
                .Cast<UIElement>()
                .ToList()
                .Where(c => c is Rectangle)
                .Cast<Rectangle>()
                .ToList() 
                .Where(c => logicBoard.Board[Grid.GetRow(c), Grid.GetColumn(c)] == null)// Are there opened gaps ?
                .Any(c => logicBoard.isPlayable(Grid.GetColumn(c), Grid.GetRow(c), playerTurn%2 != 0)); //if yes, are they playable ?
        }

        /// <summary>
        /// Termine le jeu et demande si le joueur souhaite rejouer
        /// </summary>
        public void EndGame()
        {
            var winner = logicBoard.getWhiteScore() < logicBoard.getBlackScore() ? "Black" : "White";
            System.Windows.MessageBox.Show($"{winner} has won !", "End of game");


            mainTimer.Stop();

            if (System.Windows.MessageBox.Show($"{winner} has won !\n Replay ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {

                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                RestartGame();
            }
        }

        /// <summary>
        /// Redémarre le jeu
        /// </summary>
        private void RestartGame()
        {
            logicBoard.fillBoard();
            loadFromLogicBoard();
            whitePlayer.reset();
            blackPlayer.reset();
            playerTurn = 0;
            mainTimer.Start();
        }

        /// <summary>
        /// Est-ce le jeu est est terminé ?
        /// </summary>
        /// <returns></returns>
        private bool IsGameFinished()
        {
            return (!CanPlayerPlay(playerTurn) && !CanPlayerPlay(playerTurn + 1))
                //|| logicBoard.Board.Cast<Pawn>().ToList().Count(p => p == null) == 0 
                 || whitePlayer.TotalSecondsLeft <= 0 || blackPlayer.TotalSecondsLeft <=0 ;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sauvegarde de la partie courante (Sérialisation)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            SaveFileDialog Filedialog = new SaveFileDialog();
            Filedialog.DefaultExt = ".xml";
            Filedialog.Filter = "XML documents (.xml)|*.xml";
            System.Windows.Forms.DialogResult result = Filedialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = Filedialog.FileName;
                stream = System.IO.File.Open(fileName, System.IO.FileMode.Create);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, save);
                stream.Close();

                System.Windows.MessageBox.Show("Sauvegarde réussie");
            }
            else
            {
                System.Windows.MessageBox.Show("Sauvegarde échouée, aucun fichier n'a été choisi");
            }

            
        }

        /// <summary>
        /// Chargement d'une partie sérlialisée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadClick(object sender, RoutedEventArgs e)
        {
            save = null;
            OpenFileDialog dgl = new OpenFileDialog();
            dgl.DefaultExt = ".xml";
            dgl.Filter = "XML Documents (.xml)|*.xml";

            DialogResult result = dgl.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                string file = dgl.FileName;

                stream = System.IO.File.Open(file, System.IO.FileMode.Open);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                save = (Save)bformatter.Deserialize(stream);
                stream.Close();

                logicBoard = save.Board;
                playerTurn = save.turn;

                if (playerTurn % 2 == 0)
                {
                    blackPlayer = save.CurrentPlayer;
                    whitePlayer = save.Player2;
                }
                else
                {
                    whitePlayer = save.CurrentPlayer;
                    blackPlayer = save.Player2;
                }

                System.Windows.MessageBox.Show("Chargement réussi");

                loadFromLogicBoard();
                NotifyPropertyChanged("BlackScore");
                NotifyPropertyChanged("WhiteScore");
                NotifyPropertyChanged("CurrentTurn");
            }
            else
            {
                System.Windows.MessageBox.Show("Chargement échoué, pas de fichier spécifié");
            }
        }
    }
}
