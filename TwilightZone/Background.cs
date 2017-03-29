using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwilightZone
{
    class Background
    {
        private const int rows = 3;
        private const int columns = 2;

        public Rectangle ReadRectangle(Texture2D background, GameTime gameTime)
        {
            //make this once per frame

            //frametimer == 1 when our game runs at 60 hz
            float frametimer = (float)(gameTime.TotalGameTime.TotalMilliseconds * 60 / 1000);

            const float scrollDirectionY = -2;

            int starty = (int)(scrollDirectionY * frametimer);

            Rectangle backgroundReadRectangle = new Rectangle(0,
                starty,
                background.Width * columns,
                background.Height * rows);

            return backgroundReadRectangle;
        }
    }
}
