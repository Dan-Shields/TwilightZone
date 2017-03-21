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
        public Rectangle hitbox { get; private set; }
        public int asteroidSize { get; private set; }

        private int speed = 2;

        public Asteroid(Point startingPoint, int size)
        {
            currentPosition = startingPoint;
            asteroidSize = size;
            hitbox = new Rectangle(currentPosition.X, currentPosition.Y, size, size);
        }

        public void Update()
        {
            currentPosition = new Point(currentPosition.X, currentPosition.Y + speed);
            hitbox = new Rectangle(currentPosition.X, currentPosition.Y, asteroidSize, asteroidSize);
        }
    }
}
