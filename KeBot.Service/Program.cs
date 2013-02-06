using System;
using Ninject;

namespace KeBot.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new BotsModule());
            KeBot bot = new KeBot(kernel);
            Console.WriteLine("Type help for command lists");
            while (true)
            {
                try
                {
                    string line = Console.ReadLine();
                    bot.Execute(line);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return;
        }
    }
}
