﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleApplication3
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program2
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }

        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        private static Texture2D defaultTexture;
        private static void EnsureDefaultTexture(GraphicsDevice gd)
        {
            if (defaultTexture == null)
            {
                defaultTexture = new Texture2D(gd, 1, 1);
                defaultTexture.SetData(new[] { Color.White });
            }
        }

        public static void DrawRect(this SpriteBatch sb, Vector2 center, int size, Color color)
        {
            EnsureDefaultTexture(sb.GraphicsDevice);
            sb.Draw(defaultTexture, new Rectangle(new Point((int)center.X, (int)center.Y), new Point(size, size)), color);
        }

        public static void DrawLine(this SpriteBatch sb, Vector2 start, Vector2 end, Color color, int width = 1)
        {
            EnsureDefaultTexture(sb.GraphicsDevice);

            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);

            sb.Draw(defaultTexture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    width), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0.5f), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }

    }
#endif
}
