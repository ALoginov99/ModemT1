using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    /// <summary>
    /// play space class
    /// </summary>
    public class Region
    {
        public List<int[]> Field;
        public int Width = 0;
        public int Height = 0;
        public int Points = 0;

        public Region(int width, int heigth)
        {
            Width = width;
            Height = heigth;
            Field = new List<int[]>();
            for (int i = 0; i < Height; i++)
            {
                Field.Add(new int[Width]);
            }
        }

        /// <summary>
        /// check if the figure is in the region field
        /// </summary>
        public bool CheckFigure(Figure figure)
        {
            //the borders
            if (figure.PosX < 0 || figure.PosX > Width - figure.Width ||
                figure.PosY < 0 || figure.PosY > Height - figure.Height)
                return false;
            //intersection with internal figures    
            for (int i = 0; i < figure.Height; i++)
                for (int j = 0; j < figure.Width; j++)
                    if (Field[figure.PosY + i][figure.PosX + j] == 1 && figure.Field[i, j] == 1)
                    {
                        return false;
                    }
            return true;
        }
        /// <summary>
        /// add figure to region
        /// </summary>
        public void AddFigure(Figure figure)
        {
            for (int i = 0; i < figure.Height; i++)
                for (int j = 0; j < figure.Width; j++)
                    if (figure.Field[i, j] == 1)
                    {
                        Field[figure.PosY + i][figure.PosX + j] = figure.Field[i, j];
                    }
        }

        /// <summary>
        /// display on console
        /// </summary>
        public void Draw()
        {
            Console.Clear();
            for (int i = 0; i < Height; i++)
            {
                Console.Write('|');
                for (int j = 0; j < Width; j++)
                {
                    Console.Write(Field[i][j] == 1 ? '*' : ' ');
                }
                Console.Write('|');
                Console.WriteLine();
            }
            Console.SetCursorPosition(Width + 3, 0);
            Console.Write(Points);

        }
        /// <summary>
        /// removes complete lines and adds points
        /// </summary>
        /// <returns>number of complete lines</returns>
        public bool Join()
        {
            int sum = Field.RemoveAll(line => line.All(elem => elem == 1));
            for (int i = 0; i < sum; i++)
            {
                int[] line = new int[Width];
                Field.Insert(0, line);
            }
            Points += sum;
            return sum == 0 ? false : true;
        }
    }
}
