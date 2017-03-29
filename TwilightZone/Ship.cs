using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TwilightZone
{
    class Ship
    {
        public Point currentPosition { get; private set; }
        public int health { get; private set; }
        public Rectangle hitbox { get; private set; }
        public int timeSinceDamage { get; private set; } = 60;

    private const int Speed = 4;

        public Ship()
        {
            currentPosition = new Point(TwilightZone.virtualWidth /2 - 25, TwilightZone.virtualHeight - 70);
            health = 100;
            hitbox = new Rectangle(currentPosition.X, currentPosition.Y, 50, 50);
        }

        public void Update(KeyboardState keyboardState)
        {
            string upState = Convert.ToInt16(keyboardState.IsKeyDown(Keys.Up)).ToString();
            string downState = Convert.ToInt16(keyboardState.IsKeyDown(Keys.Down)).ToString();
            string leftState = Convert.ToInt16(keyboardState.IsKeyDown(Keys.Left)).ToString();
            string rightState = Convert.ToInt16(keyboardState.IsKeyDown(Keys.Right)).ToString();
            string arrowState = upState + downState + leftState + rightState;

            switch (arrowState)
            {
                //Axial direction
                case "1000":
                case "1011":
                    currentPosition = new Point(currentPosition.X, currentPosition.Y - Speed);
                    break;
                case "0100":
                case "0111":
                    currentPosition = new Point(currentPosition.X, currentPosition.Y + Speed);
                    break;
                case "0010":
                case "1110":
                    currentPosition = new Point(currentPosition.X - Speed, currentPosition.Y);
                    break;
                case "0001":
                case "1101":
                    currentPosition = new Point(currentPosition.X + Speed, currentPosition.Y);
                    break;

                //Diagonal direction
                case "1010":
                    currentPosition = new Point(currentPosition.X - 3, currentPosition.Y - 3);
                    break;
                case "1001":
                    currentPosition = new Point(currentPosition.X + 3, currentPosition.Y - 3);
                    break;
                case "0110":
                    currentPosition = new Point(currentPosition.X - 3, currentPosition.Y + 3);
                    break;
                case "0101":
                    currentPosition = new Point(currentPosition.X + 3, currentPosition.Y + 3);
                    break;
            }
            
            //Prevent moving out of bounds
            if (currentPosition.X > TwilightZone.virtualWidth - 400)
            {
                currentPosition = new Point(TwilightZone.virtualWidth -400, currentPosition.Y);
            }
            if (currentPosition.X < 350)
            {
                currentPosition = new Point(350, currentPosition.Y);
            }
            if (currentPosition.Y > TwilightZone.virtualHeight - 50)
            {
                currentPosition = new Point(currentPosition.X, TwilightZone.virtualHeight - 50);
            }
            if (currentPosition.Y < 0)
            {
                currentPosition = new Point(currentPosition.X, 0);
            }

            hitbox = new Rectangle(currentPosition.X, currentPosition.Y, 50, 50);

            timeSinceDamage++;
        }

        public void SustainDamage(int damage)
        {
            if (timeSinceDamage <= 60) return;
            health -= damage;
            timeSinceDamage = 0;
        }

    }
}
