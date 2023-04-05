using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Audio;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using BTDevcade;
using System.Runtime.CompilerServices;

namespace DevcadeGame
{
    internal class BalloonManager
    {
        // --- Fields --- //

        
        // TODO: organize these
        private List<Vector2> Map1path;
        private List<Balloons> balloons;
        private Rectangle tileSize;
        private KeyboardState currentKB;
        private KeyboardState previousKB;
        private Texture2D balloonTexture;
        public event LoseResource takeDamage;
        public event LoseResource gainMoney;
        private SoundEffect pop;
        public List<List<BalloonWaveHelper>> roundsList;
        private double spawnTimer;
        private int round;
        private bool endRoundRewardGiven;


        // --- Properties --- //

        /// <summary>
        /// List of balloons
        /// </summary>
        public List<Balloons> Balloons { get { return balloons; } }

        public bool RoundEnded { get
            {
                if (round == 0
                    || round > roundsList.Count)
                {
                    return true;
                }
                if (spawnTimer > EndTime(roundsList[round-1])
                && balloons.Count == 0 && endRoundRewardGiven)
                {
                    return true;
                }
                return false;
            } 
        }

        public List<Vector2> Map1Path { get { return Map1path; } }

        // --- Constructor --- //

        /// <summary>
        /// Initialize the balloon manager
        /// </summary>
        /// <param name="tileSize"> size of one tile grid </param>
        /// <param name="balloons"> balloon texture </param>
        public BalloonManager(Rectangle tileSize,Texture2D balloons, SoundEffect pop)
        {
            balloonTexture = balloons;
            this.pop = pop;
            this.tileSize = tileSize;
            previousKB = new KeyboardState();
            currentKB = new KeyboardState();
            LoadRounds();
        }



        // --- Methods --- //

        /// <summary>
        /// Update all the balloons and check for keyboard input
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="window"> dimensions of the screen </param>
        public void Update(GameTime gt, Rectangle window, float gameSpeed, float sfxVolume)
        {
            //Keyboard input
            currentKB = Keyboard.GetState();

            //Spawn balloons based off round
            spawnTimer += gt.ElapsedGameTime.Milliseconds * gameSpeed;
            
            if (round > 0 && round <= roundsList.Count)
            {
                foreach (BalloonWaveHelper waveHelper in roundsList[round - 1])
                {
                    List<double> spawnTimesToRemove = new List<double>();
                    foreach (double spawnTime in waveHelper.SpawnTimes)
                    {
                        // When the time in the list occurs
                        if (spawnTime < spawnTimer)
                        {
                            SpawnBalloon(waveHelper.Health, window);
                            spawnTimesToRemove.Add(spawnTime);
                        }
                        
                    }
                    foreach (double spawnTime in spawnTimesToRemove)
                    {
                        waveHelper.SpawnTimes.Remove(spawnTime);
                    }
                }

                // Gain money after round ends
                if (spawnTimer > EndTime(roundsList[round - 1])
                    && balloons.Count == 0
                    && endRoundRewardGiven == false)
                {
                    gainMoney(100 + round);
                    endRoundRewardGiven = true;
                }
            }
            

            // Manual Controls to spawn and remove balloons
            #region manual controls
            // Manually Spawn Balloons
            if (currentKB.IsKeyDown(Keys.D1) && previousKB.IsKeyUp(Keys.D1))
            {
                SpawnBalloon(1, window);
            }
            if (currentKB.IsKeyDown(Keys.D2) && previousKB.IsKeyUp(Keys.D2))
            {
                SpawnBalloon(2, window);
            }
            if (currentKB.IsKeyDown(Keys.D3) && previousKB.IsKeyUp(Keys.D3))
            {
                SpawnBalloon(3, window);
            }
            if (currentKB.IsKeyDown(Keys.D4) && previousKB.IsKeyUp(Keys.D4))
            {
                SpawnBalloon(4, window);
            }
            if (currentKB.IsKeyDown(Keys.D5) && previousKB.IsKeyUp(Keys.D5))
            {
                SpawnBalloon(5, window);
            }

            // Manually Pop Balloons
            if (currentKB.IsKeyDown(Keys.P) && previousKB.IsKeyUp(Keys.P))
            {
                if (balloons.Count > 0)
                {
                    Balloons first = balloons[0];
                    foreach (Balloons b in  balloons)
                    {
                        if (b.DistanceTraveled > first.DistanceTraveled)
                        {
                            first = b;
                        }
                    }
                    first.Damage(1);
                }
            }
            if (currentKB.IsKeyDown(Keys.U) && previousKB.IsKeyUp(Keys.U))
            {
                if (balloons.Count > 0)
                {
                    Balloons first = balloons[0];
                    foreach (Balloons b in balloons)
                    {
                        if (b.DistanceTraveled > first.DistanceTraveled)
                        {
                            first = b;
                        }
                    }
                    first.Damage(3);
                }
            }
            #endregion

            // Update Balloons and check if popped
            List<Balloons> poppedBalloons = new List<Balloons>();
            foreach (Balloons b in balloons)
            {
                b.Update(gt, window, gameSpeed, sfxVolume);
                if (b.Health <= 0)
                {
                    poppedBalloons.Add(b);
                }
            }

            // Remove Popped Balloons
            foreach (Balloons b in poppedBalloons)
            {
                balloons.Remove(b);
            }

            // Previous Keyboard state
            previousKB = Keyboard.GetState();
        }

        /// <summary>
        /// when a balloon reaches the end sends event to remove lives
        /// </summary>
        /// <param name="damage"></param>
        private void TakeDamage(int damage)
        {
            takeDamage(damage);
            
        }

        private void MakeMoney(int amount)
        {
            gainMoney(amount);
        }

        /// <summary>
        /// Draw all the balloons
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Balloons b in balloons)
            {
                b.Draw(sb); 
            }
        }

        /// <summary>
        /// Create temporary code for balloons path
        /// </summary>
        private void initilizePath()
        {
            // HARD CODE, YEAH!
            Map1path = new List<Vector2>
            {
                new Vector2(tileSize.X * 2.5f, tileSize.Y * -0.5f),
                new Vector2(tileSize.X * 2.5f, tileSize.Y * 6.5f),
                new Vector2(tileSize.X * 4.5f, tileSize.Y * 6.5f),
                new Vector2(tileSize.X * 4.5f, tileSize.Y * 3.5f),
                new Vector2(tileSize.X * 8.5f, tileSize.Y * 3.5f),
                new Vector2(tileSize.X * 8.5f, tileSize.Y * 1.5f),
                new Vector2(tileSize.X * 10.5f, tileSize.Y * 1.5f),
                new Vector2(tileSize.X * 10.5f, tileSize.Y * 6.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 6.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 9.5f),
                new Vector2(tileSize.X * 5.5f, tileSize.Y * 9.5f),
                new Vector2(tileSize.X * 5.5f, tileSize.Y * 12.5f),
                new Vector2(tileSize.X * 9.5f, tileSize.Y * 12.5f),
                new Vector2(tileSize.X * 9.5f, tileSize.Y * 17.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 17.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 15.5f),
                new Vector2(tileSize.X * 2.5f, tileSize.Y * 15.5f),
                new Vector2(tileSize.X * 2.5f, tileSize.Y * 19.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 19.5f),
                new Vector2(tileSize.X * 7.5f, tileSize.Y * 22.5f),
                new Vector2(tileSize.X * 13.5f, tileSize.Y * 22.5f)
            };
        }
        
        /// <summary>
        /// Destroy all balloons when game ends
        /// </summary>
        public void RemoveAllBalloons()
        {
            balloons.Clear();
        }

        public void LoadRounds()
        {
            balloons = new List<Balloons>();
            initilizePath();
            roundsList = new List<List<BalloonWaveHelper>>();
            spawnTimer = 0;
            round = 0;
            endRoundRewardGiven = false;

            // read input from file
            StreamReader roundInput = new StreamReader("Content/Rounds.txt");
            string line = "";
            round = 0;

            // Add all the rounds
            while ((line = roundInput.ReadLine()) != null)
            {
                List<BalloonWaveHelper> currentWave = new List<BalloonWaveHelper>();
                string[] wave = line.Split(',');

                // Add all the waves to the round
                for (int i = 0; i < wave.Length; i++)
                {
                    string[] waveValues = wave[i].Split(":");
                    currentWave.Add(new BalloonWaveHelper(
                        int.Parse(waveValues[0]),       // # Of Balloons
                        int.Parse(waveValues[1]),       // Health
                        int.Parse(waveValues[2]),       // Start Time *milliseconds*
                        int.Parse(waveValues[3])));     // End Time *milliseconds*
                }
                roundsList.Add(currentWave);
            }

            roundInput.Close();
        }

        public void StartRound(int round)
        {
            spawnTimer = 0;
            this.round = round;
            endRoundRewardGiven = false;
        }

        public void SpawnBalloon(int balloonValue, Rectangle window)
        {
            if (balloonValue > 0)
            {
                balloons.Add(new Balloons(
                    balloonTexture, 0, 0, 26 * window.Width / 420, 30 * window.Width / 420,
                    balloonValue,
                    Map1path,
                    pop));
                balloons[balloons.Count - 1].takeDamage += TakeDamage;
                balloons[balloons.Count - 1].gainMoney += MakeMoney;
            }
        }

        /// <summary>
        /// Returns the spawn of the last balloon in the round
        /// </summary>
        /// <returns></returns>
        public double EndTime(List<BalloonWaveHelper> waveHelper)
        {
            // Calculates the time the last balloon is spawned
            double endTime = 0;
            foreach (BalloonWaveHelper b in waveHelper)
            {
                if (b.EndTime > endTime)
                {
                    endTime = b.EndTime;
                }
            }
            return endTime;
        }
    }
}
