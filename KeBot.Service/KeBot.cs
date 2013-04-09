using System;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Timers;
using Codeplex.Data;
using KeBot.Service.Utils;
using Ninject;
using Timer = System.Timers.Timer;

namespace KeBot.Service
{
    public class KeBot
    {
        private GroupmeApi _api;
        private BotEngine _engine;
        private string _lastMessageId;
        private long _lastMessageDateTime;
        public static bool Run;
        public KeBot(IKernel kernel)
        {
            _api = new GroupmeApi();
            _engine = new BotEngine(kernel);
            Run = true;
        }
        public void Execute(string line)
        {

            if (line.ToLower() == "groups")
            {
                StringBuilder builder = new StringBuilder();
                dynamic groups = DynamicJson.Parse(_api.GetGroups());
                foreach (dynamic group in groups)
                {
                    builder.AppendLine(string.Format("Id: {0}  Name: {1}", group.id, group.name));
                }
                Console.WriteLine(builder.ToString());
            }
            else if (line.ToLower().Contains("create_bot"))
            {
                var result = _api.CreateBot(line.Remove(line.ToLower().IndexOf("create_bot"), 10));
                Console.WriteLine(result);
            }
            else if (line.Contains("start"))
            {
                var command = line.Split(' ');
                if (command.Length < 3)
                {
                    Console.WriteLine("format: start groupid botid");
                }
              
                var timer = new Timer();
                timer.Elapsed += (sender, e) => OurTimerCallback(sender, e, command[1], command[2]);
                timer.Interval = int.Parse(ConfigurationManager.AppSettings.Get("pull_interval"));
                timer.Enabled = true;

                Console.WriteLine("Press 'x' to quit");
                CheckForExitKey();
                timer.Dispose();
            }
            else if (line.ToLower() == "help")
            {
                Console.WriteLine("group -- show groups");
                Console.WriteLine("create_bot groupid -- create bot for group");
                Console.WriteLine("start groupid botid -- start bot for group");
            }
            else
            {
                Console.WriteLine("No command found");
            }
        }

        public void OurTimerCallback(object source, ElapsedEventArgs e, string groupId, string botId)
        {
            if(!Run)
                return;
            var content = _api.GetMessages(groupId, _lastMessageId);
            if (string.IsNullOrEmpty(content))
                return;
            dynamic result = DynamicJson.Parse(content);
            if (string.IsNullOrEmpty(_lastMessageId))
            {
                foreach (var message in result.response.messages)
                {
                    if ((long) message.created_at > _lastMessageDateTime)
                    {
                        _lastMessageDateTime = (long) message.created_at;
                        _lastMessageId = message.id;
                    }
                }
            }
            else
            {
                foreach (var message in result.response.messages)
                {
                    if ((long)message.created_at > _lastMessageDateTime)
                    {
                        _lastMessageDateTime = (long)message.created_at;
                        _lastMessageId = message.id;
                    }
                    string returns = string.Empty;
                    if (!string.IsNullOrEmpty(message.user_id))
                        returns = _engine.Process(message);
                    if (!string.IsNullOrEmpty(returns))
                    {
                        var response = _api.BotPost(botId, returns);
                        Console.WriteLine(returns);
                        Console.WriteLine(response);
                    }
                }
            }
        }

        public static void CheckForExitKey()
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            while (true
                )
            {
                while (Console.KeyAvailable == false)
                    Thread.Sleep(250); 
                cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.X) break;
                
            }
        }
    }
}
