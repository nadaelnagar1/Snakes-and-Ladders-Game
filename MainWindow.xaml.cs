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
using System.Windows.Threading;


namespace Snakes_and_Ladders_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Rectangle landingRec;
        Rectangle player1;
        Rectangle player2;

        List<Rectangle> Moves = new List<Rectangle>();
        DispatcherTimer gameTimer = new DispatcherTimer();
        ImageBrush player1Image = new ImageBrush();
        ImageBrush player2Image = new ImageBrush();

        // default value of positions when the game loads is set up -1 for both players
        int i = -1;
        int j = -1;

         // position and current position integer for player1
        int player1Position;
        int player1CurrentPosition;

        // position and current position integer for player2
        int player2Position;
        int player2CurrentPosition;

        // this images integer will be used to show the board images
        int images = -1;

        // calculate the dice rolls in the game
        Random random = new Random();

        // two Boolean which detemrine whos turn it is 
        bool playerOneRound;
        bool playerTwoRound;

        // this integer will show the current position of players to the GUI
        int tempPos;

        public MainWindow()
        {
            InitializeComponent();
            //run the game
            SetupGame();

        }

        // this on click event is linked to the Canvas, so player is able to click anywhere on the canvas to play
        private void OnClickEvent(object sender, MouseButtonEventArgs e)
        {
             // below is the if statement thats checking if the player 1 and 2 boolean are set to false first
            if (playerOneRound == false && playerTwoRound == false)
            {
                player1Position = random.Next(1, 7);
                txtPlayer1.Content = "You rolled a " + player1Position;
                player1CurrentPosition = 0;

                if ((i + player1Position) <= 99)
                {
                    playerOneRound = true;
                    gameTimer.Start();
                }
                else
                {
                    if (playerTwoRound == false)
                    {
                        playerTwoRound = true;
                        player2Position = random.Next(1, 7);
                        txtPlayer2.Content = "Player2 rolled a " + player2Position;
                        player2CurrentPosition = 0;
                        gameTimer.Start();

                    }
                    else
                    {
                        gameTimer.Stop();
                        playerOneRound = false;
                        playerOneRound = false;
                    }
                }
            }

        }

         // this is the set up game function to set up the game board, player1 and player2
        private void SetupGame()
        {
            int leftPos = 10;
            int topPos = 600;
            int a = 0;

         // the two lines below are importing the images for player1 and player2 and attaching them to the image brush 
            player1Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player1.jpg"));
            player2Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player2.jpg"));

        // this is the main for loop where we will make the game board
            for (int i = 0; i < 100; i++)
            {
                images++;
                ImageBrush tileImages = new ImageBrush();
                tileImages.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/" + images + ".jpg"));

                Rectangle box = new Rectangle
                {
                    Height = 60,
                    Width = 60,
                    Fill = tileImages,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                box.Name = "box" + i.ToString();
                this.RegisterName(box.Name, box);
                Moves.Add(box);

                if (a == 10)
                {
                    topPos -= 60;
                    a = 30;
                }
                if (a == 20)
                {
                    topPos -= 60;
                    a = 0;
                }
                if (a > 20)
                {
                    a--;
                    Canvas.SetLeft(box, leftPos);
                    leftPos -= 60;
                }

                if (a < 10)
                {
                    a++;
                    Canvas.SetLeft(box, leftPos);
                    leftPos += 60;
                    Canvas.SetLeft(box, leftPos);

                }
                Canvas.SetTop(box, topPos);
                MyCanvas.Children.Add(box);
                }
                gameTimer.Tick += GameTimerEvent; 
                gameTimer.Interval = TimeSpan.FromSeconds(.2);

                 // set up the player rectangle
                player1 = new Rectangle
                {
                    Height = 30,
                    Width = 30,
                    Fill = player1Image,
                    StrokeThickness = 2

                };
                player2 = new Rectangle
                {
                    Height = 30,
                    Width = 30,
                    Fill = player2Image,
                    StrokeThickness = 2

                };

                MyCanvas.Children.Add(player1);
                MyCanvas.Children.Add(player2);

                MovePiece(player1, "box" + 0);
                MovePiece(player2, "box" + 0);
        }

         // this is the game timer event this event will move player1 and player2 on the board
        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (playerOneRound == true && playerTwoRound == false)
            {
                if (i < Moves.Count)
                {
                    if (player1CurrentPosition < player1Position)
                    {
                        player1CurrentPosition++;
                        i++;
                        MovePiece(player1, "box" + i);
                    }
                    else
                    {
                        playerTwoRound = true;
                        i = CheckLaddersOrSnakes(i);
                        MovePiece(player1, "box" + i);

                        player2Position = random.Next(1, 7);
                        txtPlayer2.Content = "Player2 rolled a " + player2Position;
                        player2CurrentPosition = 0;
                        tempPos = i;
                        txtPlayer1Position.Content = "Player1 is @" + (tempPos + 1);
                    }
                }
                if (i == 99)
                {
                    gameTimer.Stop();
                    MessageBox.Show("Game is Over , Player 1 Won" + Environment.NewLine + "click Ok to play again", "WE HAVE A WINNER !");
                    RestartGame();
                }
            }
            if (playerTwoRound == true)
            {
                if (j < Moves.Count)
                {
                    if (player2CurrentPosition < player2Position && (j + player2Position < 101))
                    {
                        player2CurrentPosition++;
                        j++;
                        MovePiece(player2, "box" + j);

                    }
                    else
                    {
                        playerOneRound = false;
                        playerTwoRound = false;
                        j = CheckLaddersOrSnakes(j);
                        MovePiece(player2, "box" + j);
                        tempPos = j;
                        txtPlayer2Position.Content = "Player 2 is @ " + (tempPos + 1);
                        gameTimer.Stop();
                    }
                }

                if (j == 99)
                {
                    gameTimer.Stop();
                    MessageBox.Show("Game is Over , Player 2 Won" + Environment.NewLine + "click Ok to play again" ,"WE HAVE A WINNER !");
                    RestartGame();

                }
            }
        }

        // this is the restart game function, it will set everything back to default when it runs
        private void RestartGame()
        {
            i = -1;
            j = -1;

            MovePiece(player1, "box" + 0);
            MovePiece(player2, "box" + 0);

            player1Position = 0;
            player1CurrentPosition = 0;

            player2Position = 0;
            player2CurrentPosition = 0;

            playerOneRound = false;
            playerTwoRound = false;

            txtPlayer1.Content = "You rolled a " + player1Position;
            txtPlayer1Position.Content = "Player 1 is @ 1";

            txtPlayer2.Content = "You rolled a " + player2Position;
            txtPlayer2Position.Content = "Player 2 is @ 1";

            gameTimer.Stop();



        }

        
        //check snakes or ladders function is to check if thep player has landed on the bottom of a ladder or top of the snake
        private int CheckLaddersOrSnakes(int num)
        {
            if (num == 1)
            {
                num = 37;
            }

            if (num == 6)
            {
                num = 13;
            }

            if (num == 7)
            {
                num = 30;
            }

            if (num == 14)
            {
                num = 25;
            }

            if (num == 15)
            {
                num = 5;
            }
            if (num == 20)
            {
                num = 41;
            }
            if (num == 27)
            {
                num = 83;
            }
            if (num == 35)
            {
                num = 43;
            }
            if (num == 45)
            {
                num = 24;
            }
            if (num == 48)
            {
                num = 10;
            }
            if (num == 50)
            {
                num = 66;
            }
            if (num == 61)
            {
                num = 18;
            }
            if (num == 63)
            {
                num = 59;
            }
            if (num == 70)
            {
                num = 90;
            }
            if (num == 73)
            {
                num = 52;
            }
            if (num == 77)
            {
                num = 97;
            }
            if (num == 86)
            {
                num = 93;
            }
            if (num == 88)
            {
                num = 67;
            }
            if (num == 91)
            {
                num = 87;
            }
            if (num == 94)
            {
                num = 74;
            }
            if (num == 98)
            {
                num = 79;
            }
            return num;
        }

        // this function will move the player and the opponent across the board
        private void MovePiece(Rectangle player, string posName)
        {
            foreach (Rectangle rectangle in Moves)
            {
                if (rectangle.Name == posName)
                {
                    landingRec = rectangle;
                }

            }
            Canvas.SetLeft(player, Canvas.GetLeft(landingRec) + player.Width / 2);
            Canvas.SetTop(player, Canvas.GetTop(landingRec) + player.Height / 2);


        }

    }
}
