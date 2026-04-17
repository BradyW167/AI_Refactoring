using JumpGame.Properties;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;

namespace JumpGame
{
    public partial class frmMain : Form
    {
        // Name of logged in user
        private string Username {  get; set; }
        // Connection options
        private string ConnectionString { get; set; }
        // Connection to database
        private SqlConnection Connection { get; set; }
        // Setup for singular commands
        private SqlCommand Command { get; set; }

        // Number of pixels to move horizontally per tick

        private const int PlayerSpeed = 10;
        // Number of pixels to move enemies horizontally per tick
        private const int Enemy1Speed = 6;
        private const int Enemy2Speed = 8;
        // Number of pixels to move platforms vertically per tick
        private const int MovingPlatform1Speed = 3;
        private const int MovingPlatform2Speed = 7;

        // Platform height limits
        private const int Platform1Top = 350;
        private const int Platform2Top = 160;
        private const int Platform1Bottom = 470;
        private const int Platform2Bottom = 280;

        // Length of a game tick in milliseconds
        private const int GameTickSpeed = 40;
        // Number of game ticks per second
        private const int TicksPerSecond = (int)(1000 / GameTickSpeed);

        // Starting point coordinates of the player
        private static readonly Point PlayerStart = new Point(10, 525);

        // Number of pixels to move vertically per tick
        private int JumpSpeed {  get; set; }

        // Number of pixels to decrease JumpSpeed per tick
        private int Gravity { get; set; }

        // State of groundedness
        private bool Grounded { get; set; }
        private bool LandedOnPlatform { get; set; }

        // State of jumping
        private bool Jumping { get; set; }

        // State of player movement
        private bool GoLeft { get; set; }
        private bool GoRight { get; set; }

        // State of enemy movement
        private bool Enemy1GoRight { get; set; }
        private bool Enemy2GoRight { get; set; }
        // State of platform movement
        private bool Platform1GoDown { get; set; }
        private bool Platform2GoDown { get; set; }

        // Platforms that each enemy moves on
        private Label Enemy1Platform {  get; set; }
        private Label Enemy2Platform {  get; set; }

        // Number of score points
        private int Score { get; set; }
        // Number of milliseconds passed since game start
        private int Time { get; set; }

        // Last key input for player orientation
        private bool LastKeyLeft { get; set; }
        private bool LastKeyRight { get; set;}

        // Plays audio streams
        private SoundPlayer MusicPlayer;

        public frmMain(string username, string connectionString)
        {
            InitializeComponent();

            Username = username;
            lblUsername.Text = Username;

            ConnectionString = connectionString;
            Connection = new SqlConnection(ConnectionString);
            Command = Connection.CreateCommand();

            Enemy1Platform = new Label();
            Enemy2Platform = new Label();

            MusicPlayer = new SoundPlayer();

            /* ---- YOU CAN CHANGE ALL THESE IN THE DESIGNER TAB --- */
            tmrGame.Interval = GameTickSpeed;

            picPlayer.Margin = new Padding(0);
            picEnemy1.Margin = new Padding(0);
            picEnemy2.Margin = new Padding(0);

            picPlayer.Tag = "player";
            picEnemy1.Tag = "enemy";
            picEnemy2.Tag = "enemy";
            /* ----------------------------------------------------- */

            // Reset incosistent variables for new game
            InitializeGame();
        }

        // Attempts to open a connection to a MySQL database.
        // Returns true on successful connection, false on failure
        private bool OpenConnection()
        {
            try
            {
                if (Connection != null)
                {
                    // Check if it's marked open but is actually unusable
                    if (Connection.State == ConnectionState.Open)
                    {
                        return true;
                    }
                    else
                    {
                        Connection.Dispose();
                        Connection = new SqlConnection(ConnectionString);
                    }

                    Connection.Open();
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Returning false, Unexpected error: " + ex.Message);
                return false;
            }
        }

        // Inserts input data into the Scores table
        private bool InsertScore(string username, int score, int time, DateTime date)
        {
            if (!OpenConnection())
            {
                return false;
            }

            // Parameterized query string (prevents SQL code injection) 
            string query = "INSERT INTO Scores (username,score,time,date) VALUES (@username,@score,@time,@date)";

            Command = Connection.CreateCommand();
            Command.CommandText = query;

            // Fill parameters with actual values
            Command.Parameters.AddWithValue("@username", username);
            Command.Parameters.AddWithValue("@score", score);
            Command.Parameters.AddWithValue("@time", time);
            Command.Parameters.AddWithValue("@date", date);

            // Execute command and store returned data
            int result = Command.ExecuteNonQuery();

            // If columns were modified, return true
            return result > 0;
        }

        // Resets all necessary properties for a new game
        private void InitializeGame()
        {
            ResetCharacters();
            ResetCoins();

            Score = 0;
            Time = 0;

            LastKeyLeft = false;
            LastKeyRight = true;

            PlayNewAudio(Resources.theme, true);

            tmrGame.Start();
        }

        // Resets character objects
        private void ResetCharacters()
        {
            ResetPlayer();
            ResetEnemies();
        }

        // Make all collected coins visible again
        private void ResetCoins()
        {
            foreach (Control control in this.Controls)
            {
                if (control.Tag != null)
                {
                    if (control is PictureBox && (string)control.Tag == "coin")
                    {
                        control.Visible = true;
                    }
                }
            }
        }


        // Plays input audio file from the SoundPlayer
        private void PlayNewAudio(UnmanagedMemoryStream newStream, bool loopMusic = false)
        {
            MusicPlayer.Stop();
            MusicPlayer.Stream = newStream;

            if (loopMusic)
                MusicPlayer.PlayLooping();
            else
                MusicPlayer.Play();
        }

        // Repositions player and resets movement properties
        private void ResetPlayer()
        {
            picPlayer.Location = PlayerStart;

            JumpSpeed = 0;
            Gravity = 2;

            Grounded = false;
            Jumping = false;

            GoLeft = false;
            GoRight = false;

            if (LastKeyLeft)
            {
                FlipHorizontal(picPlayer);
            }

            picPlayer.BringToFront();
        }

        // Repositons enemies to a random starting platform
        private void ResetEnemies()
        {
            Enemy1Platform = GetRandomPlatform();
            Enemy2Platform = GetRandomPlatform();

            PutEnemyOnPlatform(picEnemy1, Enemy1Platform);
            PutEnemyOnPlatform(picEnemy2, Enemy2Platform);

            // Flip image if enemy was going left
            if (!Enemy1GoRight)
            {
                FlipHorizontal(picEnemy1);
            }
            if (!Enemy2GoRight)
            {
                FlipHorizontal(picEnemy2);
            }

            Enemy1GoRight = true;
            Enemy2GoRight = true;

            // Ensures that enemy images are not behind coins
            picEnemy1.BringToFront();
            picEnemy2.BringToFront();
        }

        // Returns a random platform as a Label
        private Label GetRandomPlatform()
        {
            Random rand = new Random();

            // Define the possible platform name numbers
            int[] platformNumbers = { 2, 3, 5, 7, 8 };

            // Select a random index from 0 to platformNumbers.Length
            int randIndex = rand.Next(platformNumbers.Length);

            // Get the platform name using the random index
            string randomPlatformName = $"lblPlatform{platformNumbers[randIndex]}";

            if (this.Controls[randomPlatformName] is Label randomPlatform)
                return randomPlatform;
            else
                return new Label();
        }

        // Positions enemy on input platform
        private void PutEnemyOnPlatform(PictureBox enemy, Label platform)
        {
            int top = platform.Top - enemy.Height;

            int left = platform.Left + enemy.Width;

            enemy.Location = new Point(left, top);
        }

        // User input handler for key presses
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                GoLeft = true;

                if (LastKeyRight == true)
                {
                    FlipHorizontal(picPlayer);
                }

                LastKeyRight = false;
                LastKeyLeft = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                GoRight = true;

                if (LastKeyLeft == true)
                {
                    FlipHorizontal(picPlayer);
                }

                LastKeyRight = true;
                LastKeyLeft = false;
            }

            // If (key hit is space or up arrow) and player is grounded
            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Up) && Grounded && !Jumping)
            {
                Jumping = true;
            }
        }

        // User input handler for key releases
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                GoLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                GoRight = false;
            }
            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Up) && Jumping)
            {
                Jumping = false;
            }
        }

        // Game tick function, runs every (GameTickSpeed) milliseconds
        private void tmrGame_Tick(object sender, EventArgs e)
        {
            UpdateTime();
            UpdateScore();

            MoveImages();

            // Reset landed state
            LandedOnPlatform = false;

            // Loop through all controls in this game form
            foreach (Control control in this.Controls)
            {
                // Check control.Tag type (controls with tags are game objects with collision)
                if (control.Tag is string tagValue)
                {
                    CheckCollision(control, tagValue);
                }
            }

            // After all other checks, update grounded boolean
            // Needed for moving platforms
            if (!LandedOnPlatform)
            {
                Grounded = false;
            }
        }

        private void UpdateTime()
        {
            lblTime.Text = "Time: " + (Time / TicksPerSecond);
            Time += 1;
        }

        private void UpdateScore()
        {
            lblScore.Text = "Score: " + Score;
        }

        // Runs movement handling for all moving images
        private void MoveImages()
        {
            CheckPlayerMovement();

            Enemy1GoRight = MoveEnemy(picEnemy1, Enemy1Speed, Enemy1Platform, Enemy1GoRight);
            Enemy2GoRight = MoveEnemy(picEnemy2, Enemy2Speed, Enemy2Platform, Enemy2GoRight);

            Platform1GoDown = MovePlatform(lblPlatform4, MovingPlatform1Speed, Platform1GoDown, Platform1Top, Platform1Bottom);
            Platform2GoDown = MovePlatform(lblPlatform6, MovingPlatform2Speed, Platform2GoDown, Platform2Top, Platform2Bottom);
        }

        // Handles player movement
        private void CheckPlayerMovement()
        {
            if (GoLeft)
            {
                // Shift player by 'Speed' number of pixels
                picPlayer.Left -= PlayerSpeed;

                // If player goes past the left bounds of the screen
                if (picPlayer.Left < 0)
                {
                    picPlayer.Left = 0;
                    GoLeft = false;
                }
            }

            // If player is moving right...
            if (GoRight)
            {
                // Shift player by 'Speed' number of pixels
                picPlayer.Left += PlayerSpeed;

                // If player goes past the right bounds of the screen
                if (picPlayer.Right > this.ClientSize.Width)
                {
                    picPlayer.Left = this.ClientSize.Width - picPlayer.Width;
                    GoRight = false;
                }
            }

            // If beginning to jump
            if (Jumping && Grounded)
            {
                // Sets intial jump speed
                JumpSpeed = -22;
                Grounded = false;
            }

            // If mid-air
            if (!Grounded)
            {
                // Apply gravity each tick
                JumpSpeed += Gravity;

                // Cap fall speed
                if (JumpSpeed > 22) { JumpSpeed = 22; }

                // Move the player vertically
                picPlayer.Top += JumpSpeed;
            }
            else
            {
                JumpSpeed = 0;
            }

            // If player reaches top of screen
            if (picPlayer.Top < 0)
            {
                picPlayer.Top = 0;
                JumpSpeed = 0;
            }

            // If player falls off bottom of screen
            else if (picPlayer.Top > this.ClientSize.Height)
            {
                PlayerDeath();
            }
        }

        // Handles enemy movement
        private bool MoveEnemy(PictureBox enemy, int enemySpeed, Label enemyPlatform, bool goRight)
        {
            if (goRight)
                enemy.Left += enemySpeed;
            else
                enemy.Left -= enemySpeed;

            // If enemy reaches left side of platform, go right
            if (enemy.Left < enemyPlatform.Left)
            {
                goRight = true;
                FlipHorizontal(enemy);
            }
            // If enemy reaches right side of platform, go left
            else if (enemy.Right > enemyPlatform.Right)
            {
                goRight = false;
                FlipHorizontal(enemy);
            }

            return goRight;
        }

        // Handles moving platform movement
        private bool MovePlatform(Label platform, int platformSpeed, bool goDown, int topLimit, int bottomLimit)
        {
            if (goDown)
                platform.Top += platformSpeed;
            else
                platform.Top -= platformSpeed;

            if (platform.Top < topLimit)
                goDown = true;
            else if (platform.Top > bottomLimit)
                goDown = false;

                return goDown;
        }

        private void CheckCollision(Control control, string tagValue)
        {
            if (tagValue == "enemy")
            {
                EnemyCollisionCheck(control);
            }

            if (tagValue == "coin")
            {
                CoinCollisionCheck(control);
            }

            if (tagValue == "platform" || tagValue == "movingPlatform")
            {
                PlatformCollisionCheck(control);
            }
            if (tagValue == "door")
            {
                DoorCollisionCheck(control);
            }
        }

        private void EnemyCollisionCheck(Control enemy)
        {
            if (picPlayer.Bounds.IntersectsWith(enemy.Bounds))
            {
                PlayerDeath();
            }
        }

        private void CoinCollisionCheck(Control coin)
        {
            if (picPlayer.Bounds.IntersectsWith(coin.Bounds) && coin.Visible)
            {
                coin.Visible = false;
                Score += 1;
            }
        }

        // Returns true when player is grounded on top of a platform, false when not
        private void PlatformCollisionCheck(Control platform)
        {
            if (picPlayer.Bounds.IntersectsWith(platform.Bounds))
            {
                if (picPlayer.Bounds.IntersectsWith(platform.Bounds))
                {
                    Rectangle plat = platform.Bounds;

                    // Additional pixels added to collision checks
                    // Prevents player image from bouncing while on platforms
                    const int SafetyThreshold = 5;

                    // Landing on top of platform
                    if (picPlayer.Bottom >= plat.Top - 5 &&
                        picPlayer.Top < plat.Top &&
                        JumpSpeed >= 0)
                    {
                        LandedOnPlatform = true;
                        Grounded = true;
                        JumpSpeed = 0;
                        picPlayer.Top = plat.Top - picPlayer.Height + 1;
                    }
                    // Hitting platform from the sides
                    // Additional
                    else if (picPlayer.Right > plat.Left && picPlayer.Left < plat.Left && picPlayer.Bottom > plat.Top + SafetyThreshold)
                    {
                        picPlayer.Left = plat.Left - picPlayer.Width;
                        GoRight = false;
                    }
                    else if (picPlayer.Left < plat.Right && picPlayer.Right > plat.Right && picPlayer.Bottom > plat.Top + SafetyThreshold)
                    {
                        picPlayer.Left = plat.Right;
                        GoLeft = false;
                    }
                    // Hitting platform from below
                    else if (picPlayer.Top < plat.Bottom && picPlayer.Bottom > plat.Bottom)
                    {
                        JumpSpeed = 5; // small bounce back
                        picPlayer.Top = plat.Bottom;
                    }
                }
            }
        }

        private void DoorCollisionCheck(Control door)
        {
            if (picPlayer.Bounds.IntersectsWith(door.Bounds))
            {
                Victory();
            }
        }

        // Horizontally flips the input character PictureBox
        private void FlipHorizontal(PictureBox character)
        {
            character.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            character.Refresh();
        }

        // Ends game with victory
        private void Victory()
        {
            tmrGame.Stop();
            PlayNewAudio(Resources.smb_gameover);

            int TimeInSeconds = (int)(Time / TicksPerSecond);

            InsertScore(Username, Score, TimeInSeconds, DateTime.Now);

            RestartPrompt(true);
        }

        // Ends game with player death
        private void PlayerDeath()
        {
            tmrGame.Stop();
            PlayNewAudio(Resources.smb_mariodie);

            RestartPrompt(false);
        }

        // Prompts player about their win or loss and asks to view scores or play again
        private void RestartPrompt(bool wonGame)
        {
            string message = string.Empty;
            string caption = "Play Again?";

            if (wonGame)
                message = $"You Won! Score:{Score}\n\nView Scores?";
            else
                message = "You Died!\n\nPlay Again?";

            // If user hits yes to play again
            if (MessageBox.Show(message, caption,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (wonGame)
                {
                    this.Hide();
                    frmScores scoreForm = new frmScores(Username, ConnectionString);
                    scoreForm.Show();
                    this.Dispose();
                }
                else
                {
                    InitializeGame();
                }
            }
            // Else user does want to view scores or play again
            else
            {
                Application.Exit();
            }
        }

        // When logout label is clicked
        private void lblLogout_Click(object sender, EventArgs e)
        {
           Logout();
        }

        // When form is closed (Windows X button)
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Switches to login form
        private void Logout()
        {
            this.Hide();

            // Get a reference to the login form (assuming it's still alive)
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is frmLogin loginForm)
                {
                    loginForm.Logout();
                    loginForm.Show();
                    break;
                }
            }

            this.MusicPlayer.Stop();
            this.Dispose();
        }
    }
}
