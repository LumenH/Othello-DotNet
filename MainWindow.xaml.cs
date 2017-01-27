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
        private LogicBoard logicBoard;

        private readonly SolidColorBrush playableColor = new SolidColorBrush(Color.FromRgb(0, 111, 111));
        private readonly SolidColorBrush regularColor = new SolidColorBrush(Colors.Green);

        private List<Ellipse> pawns = new List<Ellipse>();

        private System.IO.Stream stream;

        public MainWindow()
        {
            InitializeComponent();
            
            logicBoard = new LogicBoard();
            logicBoard.fillFakeBoard();
            loadFromLogicBoard();

            UpdateScore();
        }

        private void UpdateScore()
        {
            nbPawnPlayer2.Content = Convert.ToString(logicBoard.getBlackScore());
            nbPawnPlayer1.Content = Convert.ToString(logicBoard.getBlackScore());
        }

        private void mouseDown(object sender, MouseButtonEventArgs e)
        {

            var element = (UIElement)e.Source;
            var c = Grid.GetColumn(element);
            var r = Grid.GetRow(element);

            var playable = logicBoard.isPlayable(c, r, playerTurn % 2 != 0);

            if (playable)
                logicBoard.playMove(c, r, playerTurn%2 != 0);

            loadFromLogicBoard();
            playerTurn++;
            UpdateScore();
        }

        private void loadWindow(object sender, RoutedEventArgs e)
        {

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
                            Fill = value.Color == Pawn.Colors.Withe ? Brushes.White : Brushes.Black
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

        private void saveClick(object sender, RoutedEventArgs e)
        {
            stream = System.IO.File.Open("save.xml", System.IO.FileMode.Create);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            bformatter.Serialize(stream, logicBoard);
            stream.Close();

            MessageBox.Show("Sauvegarde réussie");
        }

        private void loadClick(object sender, RoutedEventArgs e)
        {
            logicBoard = null;

            stream = System.IO.File.Open("save.xml", System.IO.FileMode.Open);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            logicBoard = (LogicBoard)bformatter.Deserialize(stream);
            stream.Close();

            MessageBox.Show("Load réussi");

            loadFromLogicBoard();
        }
    }
}
