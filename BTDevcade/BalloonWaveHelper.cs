using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTDevcade
{
    internal class BalloonWaveHelper
    {
        // --- Fields --- //

        private List<double> spawnTimes;
        private int health;
        private int endTime;


        // --- Properties --- //
        
        /// <summary>
        /// Returns all the times to spawn the balloon
        /// </summary>
        public List<double> SpawnTimes { get { return spawnTimes; } set { spawnTimes = value; } }

        /// <summary>
        /// The health of the balloons to spawn
        /// </summary>
        public int Health { get { return health; } }

        /// <summary>
        /// Return the last balloon spawned
        /// </summary>
        public int EndTime { get { return endTime; } set { endTime = value; } }
        
        // --- Constructor --- //

        public BalloonWaveHelper(int numberOfBalloons, int health, int startTime, int endTime)
        {
            this.health = health;
            this.endTime = endTime;
            spawnTimes = new List<double>();
            for (int i = 0; i < numberOfBalloons; i++)
            {
                if (numberOfBalloons > 1)
                {
                    spawnTimes.Add(((i * (endTime - startTime)) / (numberOfBalloons - 1)) + startTime);
                } else
                {
                    spawnTimes.Add(startTime);
                }
            }
        }

    }
}
