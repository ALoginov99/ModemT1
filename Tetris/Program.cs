using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tetris
{
    class Program
    {
        static Highscores Highscores = new Highscores();

        static Mutex Mutex = new Mutex();
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

            Console.WriteLine("Рекорды");
            Highscores.Print();
            Console.WriteLine("Нажмите для продолжения");
            Console.ReadKey();
            Console.Clear();

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
            Console.Clear();
            TimeSleep = 60*1000/timeSleep;
            Region = new Region(width, heigth);
        }

        static void Main(string[] args)
        {
            Initial();

            Region.Draw();
            Region.DrawBorder();
            Figure.Draw();

            Task taskPlay = new Task(Play);
            taskPlay.Start();
            Task taskDown = new Task(Down);
            taskDown.Start();

            taskPlay.Wait();
            taskDown.Wait();

            SetHighscores();
        }
        /// <summary>
        /// keystroke tracking method
        /// </summary>
        public static void Play()
        {
            while (check)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                Mutex.WaitOne();
                if (!check)
                    break;
                Figure figure = (Figure)Figure.Clone();
                switch (key.Key)
                {
                    case (ConsoleKey.RightArrow):
                        figure.PosX += 1;
                        break;
                    case (ConsoleKey.LeftArrow):
                        figure.PosX -= 1;
                        break;
                    case (ConsoleKey.DownArrow):
                        figure.PosY += 1;
                        break;
                    case (ConsoleKey.Spacebar):
                        figure = figure.Rotation();
                        break;
                    case (ConsoleKey.Escape):
                        check = false;
                        break;
                    default:
                        break;
                }
                //rollback changes if necessary
                if (Region.CheckFigure(figure))
                {
                    Figure.Clear();
                    Figure = figure;
                    Figure.Draw();
                }
                Mutex.ReleaseMutex();
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
                Mutex.WaitOne();
                if (!Region.CheckFigure(Figure))
                {
                    check = false;
                    Console.Clear();
                    Console.WriteLine("Игра окончена, ваш счет:{0}",Region.Points);
                    Mutex.ReleaseMutex();
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
                Mutex.ReleaseMutex();
            }
        }

        static void SetHighscores()
        {
            if(Highscores.CheckIsTop10(Region.Points))
            {
                Console.WriteLine("Поздравляем!!! Вы попали в топ 10 лучших игроков данного тетриса, пожалуйста введите свое имя");
                string name = Console.ReadLine();
                Highscores.AddInTop10(name,Region.Points);
            }
        }
    }
}
