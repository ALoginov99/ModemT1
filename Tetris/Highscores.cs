using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Tetris
{
    class Highscores
    {
        struct Player
        {
            public string Name;
            public int Points;
        }
        private List<Player> Players = new List<Player>();

        public Highscores()
        {
            if (!File.Exists(@"D:\HighscoresDB.db"))
            {
                SQLiteConnection.CreateFile(@"D:\HighscoresDB.db");
            }
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=D:\HighscoresDB.db; Version=3;"))
            {
                string commandText =
                    "CREATE TABLE IF NOT EXISTS [Highscores] ( [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,[Name] varchar(20), [Points] INTEGER)";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery();

                Command = new SQLiteCommand
                {
                    Connection = Connect,
                    CommandText = @"SELECT * FROM [Highscores] order by [Points] desc"
                };
                SQLiteDataReader sqlReader = Command.ExecuteReader();
                while (sqlReader.Read())
                {
                    Players.Add(new Player { Name = sqlReader["Name"].ToString(),Points = int.Parse(sqlReader["Points"].ToString()) });
                }

                Connect.Close();
            }
        }
        public bool CheckIsTop10(int points)
        {
            if (Players.Count<10 || Players.Any(a => a.Points < points))
                return true;
            else
                return false;
        }
        public void AddInTop10(string name, int points)
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=D:\HighscoresDB.db; Version=3;"))
            {
                SQLiteCommand Command;
                Connect.Open();
                if (Players.Count >= 10)
                {
                    Command = new SQLiteCommand
                    {
                        Connection = Connect,
                        CommandText = @"DELETE [Highscores] WHERE [id] = (select top 1 [id] from [Highscores] order by [Points])"
                    };
                    Command.ExecuteNonQuery();
                }
                Command = new SQLiteCommand
                {
                    Connection = Connect,
                    CommandText = string.Format(@"insert into [Highscores]([Name],[Points]) values('{0}',{1})",name,points)
                };
                Command.ExecuteNonQuery();
                Connect.Close();
            }
        }
        public void Print()
        {
            if (Players.Count == 0)
                Console.WriteLine("Пока рекордов нет");
            Players.ForEach(a=> Console.WriteLine("Имя:{0} Счет:{1}",a.Name,a.Points));
        }
    }
}
