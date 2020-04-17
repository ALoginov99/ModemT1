using System;
using System.Linq;

namespace Tetris
{
    /// <summary>
    /// game figure class
    /// </summary>
    public class Figure
    {
        public int[,] Field;
        public int Width = 0;
        public int Height = 0;
        public int PosX = 0;
        public int PosY = 0;

        public Figure()
        {
            Reset();
        }

        /// <summary>
        /// make a new Figure
        /// </summary>
        public void Reset()
        {
            Field = RandomFigure();
            PosX = 0;
            PosY = 0;
        }

        /// <summary>
        /// generate figure
        /// </summary>
        public int[,] RandomFigure()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int[] random = new int[4];
            for (int i = 0; i < 4; i++)
            {
                random[i] = r.Next(2);
            }
            Width = random.Sum() + 1;
            Height = 6 - Width;
            int[,] figure = new int[Height, Width];
            int x = 0, y = 0;
            for (int i = 0; i < 4; i++)
            {
                if (random[i] == 0)
                    figure[x++, y] = 1;
                else
                    figure[x, y++] = 1;
            }
            figure[x, y] = 1;
            return figure;
        }

        /// <summary>
        /// display on console
        /// </summary>
        public void Draw()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    if (Field[i, j] == 1)
                    {
                        Console.SetCursorPosition(PosX + j + 1, PosY + i);
                        Console.Write('*');
                    }
        }
        /// <summary>
        /// clear on console
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    if (Field[i, j] == 1)
                    {
                        Console.SetCursorPosition(PosX + j + 1, PosY + i);
                        Console.Write(' ');
                    }
        }
        /// <summary>
        /// rotates and returns the figure(current does not change)
        /// </summary>
        public Figure Rotation()
        {
            Figure figure = new Figure();
            figure.Field = new int[Width, Height];

            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    figure.Field[i, j] = Field[Height - j - 1, i];

            figure.Width = Height;
            figure.Height = Width;
            figure.PosX = PosX;
            figure.PosY = PosY;

            return figure;
        }
    }
}
