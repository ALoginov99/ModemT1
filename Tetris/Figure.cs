using System;

namespace Tetris
{
    /// <summary>
    /// game figure class
    /// </summary>
    public class Figure : ICloneable
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
            int[,] field = new int[,] { };
            switch (r.Next(7))
            {
                case 0: field = new int[,] { { 1, 1, 1, 1 } }; break;//I
                case 1: field = new int[,] { { 1, 1, 1 }, { 1, 0, 0 } }; break;//L
                case 2: field = new int[,] { { 1, 0, 0 }, { 1, 1, 1 } }; break;//J
                case 3: field = new int[,] { { 1, 1 }, { 1, 1 } }; break;//O
                case 4: field = new int[,] { { 0, 1, 1 }, { 1, 1, 0 } }; break;//S
                case 5: field = new int[,] { { 1, 1, 0 }, { 0, 1, 1 } }; break;//z
                case 6: field = new int[,] { { 0, 1, 0 }, { 1, 1, 1 } }; break;//z
            }
            Height = field.GetLength(0);
            Width = field.GetLength(1);
            return field;
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

        public object Clone()
        {
            return new Figure()
            {
                Field = this.Field,
                Width = this.Width,
                Height = this.Height,
                PosX = this.PosX,
                PosY = this.PosY
            };
        }
    }
}
