using System;
using System.Threading;

namespace Tetris
{
    class Program
    {
        static Mutex mutex = new Mutex();
        static Figure Figure = new Figure();
        static Region Region;
        static int TimeSleep = 1000;
        static bool check = true;

        /// <summary>
        /// welcome and initialization of initial parameters
        /// </summary>
        public static void Initial()
        {
            Console.CursorVisible = false;

            Console.WriteLine("Добро пожаловать в игру тетрис!!");
            Console.WriteLine("Перед началом прочитайте правила и введите параметры игры");
            Console.WriteLine("Управление осуществляется клавишами 'стрелка влево','стелка вправо','стрелка вниз'");
            Console.WriteLine("'пробел'(для поворота фигуры)");
            Console.Write("Ширина игрового поля ->");
            int width,heigth,timeSleep;
            int.TryParse(Console.ReadLine(), out width);
            Console.Write("Высота игрового поля ->");
            int.TryParse(Console.ReadLine(), out heigth);
            Console.Write("Скорость падения фигур(клеток в минуту)->");
            int.TryParse(Console.ReadLine(), out timeSleep);
            TimeSleep = timeSleep*1000/60;
            Region = new Region(width, heigth);
        }

        static void Main(string[] args)
        {
            Initial();

            Region.Draw();
            Figure.Draw();

            Thread threadPlay = new Thread(Play);
            threadPlay.Start();
            Thread threadDown = new Thread(Down);
            threadDown.Start();

            Console.ReadKey();
        }
        /// <summary>
        /// keystroke tracking method
        /// </summary>
        public static void Play()
        {
            while (check)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                mutex.WaitOne();
                int posX = Figure.PosX;
                int posY = Figure.PosY;
                Figure.Clear();
                switch (key.Key)
                {
                    case (ConsoleKey.RightArrow):
                        Figure.PosX += 1;
                        break;
                    case (ConsoleKey.LeftArrow):
                        Figure.PosX -= 1;
                        break;
                    case (ConsoleKey.DownArrow):
                        Figure.PosY += 1;
                        break;
                    case (ConsoleKey.Spacebar):
                        Figure figure = Figure.Rotation();
                        if (Region.CheckFigure(Figure))
                        {
                            Figure = figure;
                        }
                        break;
                    case (ConsoleKey.Escape):
                        check = false;
                        break;
                    default:
                        break;
                }
                //rollback changes if necessary
                if (!Region.CheckFigure(Figure))
                {
                    Figure.PosX = posX;
                    Figure.PosY = posY;
                }
                else
                {
                    Figure.Draw();
                }
                mutex.ReleaseMutex();
            }
        }
        /// <summary>
        /// the fall of the figure
        /// </summary>
        public static void Down()
        {
            while (check)
            {
                Thread.Sleep(TimeSleep);
                mutex.WaitOne();
                if (!Region.CheckFigure(Figure))
                {
                    check = false;
                    Console.Clear();
                    Console.WriteLine("Игра окончена, ваш счет:{0}",Region.Points);
                    break;
                }
                Figure.Clear();
                Figure.PosY += 1;
                if (!Region.CheckFigure(Figure))
                {
                    Figure.PosY -= 1;
                    Region.AddFigure(Figure);

                    if(Region.Join())
                    {
                        Region.Draw();
                    }
                    else
                    {
                        Figure.Draw();
                    }
                    Figure.Reset();
                }
                Figure.Draw();
                mutex.ReleaseMutex();
            }
        }
    }
}
