using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TwilightZone
{
    public class Laser
    {
        public Point currentPosition { get; private set; }
        public Rectangle hitbox { get; private set; }

        private int speed = 5;

        public Laser(Point startingPoint)
        {
            currentPosition = startingPoint;
            hitbox = new Rectangle(currentPosition.X, currentPosition.Y, 10, 15);
        }

        public void Update()
        {
            currentPosition = new Point(currentPosition.X, currentPosition.Y - speed);
            hitbox = new Rectangle(currentPosition.X, currentPosition.Y, 10, 15);
        }

        public void TestCollision()
        {

        }
    }
}
