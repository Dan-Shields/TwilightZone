using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TwilightZone
{
    class Asteroid
    {
        public Point currentPosition { get; private set; }

        private int speed = 2;

        public Asteroid(Point startingPoint)
        {
            currentPosition = startingPoint;
        }

        public void Tick()
        {
            currentPosition = new Point(currentPosition.X, currentPosition.Y + speed);
        }
    }
}
