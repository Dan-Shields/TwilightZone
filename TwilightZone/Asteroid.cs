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
        public Circle hitbox { get; private set; }
        public int asteroidSize { get; }

        private const int Speed = 2;

        public Asteroid(Point startingPoint, int size)
        {
            currentPosition = startingPoint;
            asteroidSize = size;
            hitbox = new Circle(currentPosition.X, currentPosition.Y, asteroidSize);
        }

        public void Update()
        {
            currentPosition = new Point(currentPosition.X, currentPosition.Y + Speed);
            hitbox = new Circle(currentPosition.X, currentPosition.Y, asteroidSize);
        }
    }
}
