using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;  
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceRace2
{
    public partial class Form1 : Form
    {
        string state = "main menu";
        
        static int player1Score = 0;
        static int player2Score = 0;
        static bool gameOver = false;

        int playerSpeed = 5;

        private Stopwatch stopwatch;
        bool wDown = false;
        bool aDown = false;
        bool sDown = false;
        bool dDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool leftDown = false;
        bool rightDown = false;
        Rectangle player1 = new Rectangle(200, 230, 28, 60);
        Rectangle player2 = new Rectangle(450, 230, 28, 60);
        Rectangle asteroid = new Rectangle();
        List <Rectangle> asteroidList = new List<Rectangle>();
        List <int> asteroidSpeedList = new List<int>();
        Random random = new Random();




        public Form1()
        {
            InitializeComponent();

            InitializeGame();

        }
        private void InitializeGame()
        {

            stopwatch = new Stopwatch();
            stopwatch.Start();

            timer1.Start();
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
                case Keys.Space:
                    if (state == "main menu")
                    {
                        state = "playing";
                        timer1.Enabled = true;

                        label3.Visible = false;
                        pressSpaceBarToStart.Visible = false;
                        label1.Visible = false;
                        label2.Visible = false;
                    }
                    break;
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (state == "playing")
            {
                // Draw oval rocket ships for players
                e.Graphics.FillEllipse(Brushes.Red, player1.Left, player1.Top, player1.Width, player1.Height);
                e.Graphics.FillEllipse(Brushes.Blue, player2.Left, player2.Top, player2.Width, player2.Height);

                for (int i = 1; i < asteroidList.Count; i++)
                {
                    e.Graphics.FillEllipse(Brushes.White, asteroidList[i]);
                }
                string scoreText = $"Player 1: {player1Score}   Player 2: {player2Score}";
                string timeText = $"Time: {stopwatch.Elapsed.TotalSeconds:F1}s";

                e.Graphics.DrawString(scoreText, Font, Brushes.White, (Width - e.Graphics.MeasureString(scoreText, Font).Width) / 2, 0);
                e.Graphics.DrawString(timeText, Font, Brushes.White, (Width - e.Graphics.MeasureString(timeText, Font).Width) / 2, 20);
            }
            else
            {
                label3.Visible = true;
                pressSpaceBarToStart.Visible = true;
                label1.Visible = false;
                label2.Visible = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        


            if (upArrowDown == true)
            {
                player2.Y -= playerSpeed;
            }
            if (downArrowDown == true && player2.Y < 230)
            {
                player2.Y += playerSpeed;
            }
            if (wDown == true)
            {
                player1.Y -= playerSpeed;
            }
            if (sDown == true && player1.Y < 230)
            
            {
                player1.Y += playerSpeed;
            }

            int randNum = random.Next(1, 101);
            if (randNum <= 10) // Adjust the probability as needed (10% chance in this case)
            {
                int asteroidY = random.Next(0, ClientSize.Height - 30); // Generate a random Y position within the screen height

                int asteroidX;
                int asteroidSpeed;

                randNum = random.Next(1, 101);
                if (randNum <= 50)
                {
                    asteroidX = -15;
                    asteroidSpeed = 10;
                }
                else
                {
                    asteroidX = ClientSize.Width - 15;
                    asteroidSpeed = -playerSpeed;
                }

                int asteroidWidth = 15;
                int asteroidHeight = 15;
                Rectangle newAsteroid = new Rectangle(asteroidX, asteroidY, asteroidWidth, asteroidHeight);
                asteroidList.Add(newAsteroid);
                asteroidSpeedList.Add(asteroidSpeed);
            }

            for (int i = 0; i < asteroidList.Count; i++)
            {
                Rectangle asteroid = asteroidList[i];
                asteroid.X += asteroidSpeedList[i]; // Adjust the falling speed of the asteroids

                if (player1.IntersectsWith(asteroid))
                {
                    player1.X = 200; // Reset player 1 to the start position
                    player1.Y = 230;
                }
                else if (player2.IntersectsWith(asteroid))
                {
                    player2.X = 450; // Reset player 2 to the start position
                    player2.Y = 230;
                }

                asteroidList[i] = asteroid;

                if (asteroid.Y > ClientSize.Height)
                {
                    asteroidList.RemoveAt(i);
                    asteroidSpeedList.RemoveAt(i);
                    i--;
                }
            }

            if (player1.Y <= 0)
            {
                player1Score++;
                player1.X = 200; // Reset player 1 to the start position
                player1.Y = 230;
                if (player1Score >= 10)
                {
                    label1.Text = "Player 1 wins!";
                    gameOver = true;
                    timer1.Stop();
                }
            }
            if (player2.Y <= 0)
            {
                player2Score++;
                player2.X = 450; // Reset player 2 to the start position
                player2.Y = 230;
                if (player2Score >= 10)
                {
                    label2.Text = "Player 2 wins!";
                    gameOver = true;
                    timer1.Stop();
                }
            }

            if (gameOver || stopwatch.Elapsed.TotalSeconds >= 60)
            {
                GameOverScreen();
            }



            Refresh();
        }
        private void GameOverScreen()
        {
            if (player1Score > player2Score)
            {
                label1.Visible = true;
                label1.Text = "Player 1 wins!";
            }
            else if (player1Score < player2Score)
            {
                label2.Visible = true;
                label2.Text = "PLayer 2 wins!";
            }
            timer1.Stop();
            
            MessageBox.Show("Game Over");
            state = "main menu";
            Refresh();
        }
      

    }


}
