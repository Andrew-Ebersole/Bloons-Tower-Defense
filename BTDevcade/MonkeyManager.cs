using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DevcadeGame
{
internal class MonkeyManager
    {
        // --- Fields --- //

        private List<Monkey> monkeys;
        private enum monkeyType
        {
            Dart,
            Tack,
            Sniper,
            Super,
        }


        // --- Properties --- //





        // --- Constructor --- //

        public MonkeyManager()
        {
        }



        // --- Methods --- //

        public void Update(GameTime gt, Rectangle windowDimensions)
        {
            foreach(Monkey m in monkeys)
            {
                m.Update(gt, windowDimensions);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach(Monkey m in monkeys)
            {
                m.Draw(sb);
            }
        }

    }
}
