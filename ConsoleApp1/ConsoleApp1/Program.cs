﻿using Dapper;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Test();
           
            int numberOfTasks = 300;
            Console.WriteLine($"task count: {numberOfTasks}");

            Task[] tasks = new Task[numberOfTasks];

            for (int i = 0; i < numberOfTasks; i++)
            {
                string taskName = $"Task{i}";
                tasks[i] = Task.Run(() =>
                {
                    Thread.CurrentThread.Name = taskName;
                    for (int i = 0; i < 10 * 10000; i++)
                    {
                        Test();
                    }
                });
            }

            Task.WaitAll(tasks);

            Console.WriteLine("the end");
            Console.ReadLine();

        }



        static void Test()
        {
            try
            {
                string sql = @"show PROCESSLIST;";

                string conStr = "Server=dbk.db.cc;Port=4304;Stmt=;Database=dmp; User=mib_dmp_n6;Password=5tgbNHY^BGT%X;Pooling=true;MinPoolSize=200;MaxPoolSize=1000;AllowPublicKeyRetrieval=true";
                using var con = new MySqlConnector.MySqlConnection(conStr);
                var dogList = con.QueryAsync<Dog>(sql).Result.ToList();
                //Console.WriteLine($"{DateTime.Now}-{Thread.CurrentThread.Name}：{dogList.Count()}");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Object reference not set to an instance of an object"))
                {
                    Console.WriteLine($"{DateTime.Now}-{Thread.CurrentThread.Name}：{ex.Message} {ex.StackTrace}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"{DateTime.Now}-{Thread.CurrentThread.Name}：{ex.InnerException.Message} {ex.InnerException.StackTrace}");
                    }
                }
            }
        }

        public class Dog
        {

            public int Id { get; set; }
        }


    }
}