using System;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace KeBot.Service
{
    internal class BotEngine
    {
        private IKernel _kernel;

        public BotEngine(IKernel kernel)
        {
            _kernel = kernel;
        }

        public string Process(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var bots = _kernel.GetAll<IBot>();
            StringBuilder builder = new StringBuilder();
            Parallel.ForEach<IBot>(bots, async bot => builder.Append(await ProccessInternal(input, bot)));
            return builder.ToString();
        }

        private async Task<string> ProccessInternal(string input, IBot bot)
        {
            string result = string.Empty;

            try
            {
                result = bot.Process(input);
            }
            catch (Exception exception)
            {

                return "Error running moduel " + bot.GetType() + " : " + exception;
            }

            return result;
        }
    }

    internal interface IBot
    {
        string Process(string input);
    }
}