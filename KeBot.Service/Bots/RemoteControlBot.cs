using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBot.Service.Bots
{
    public class RemoteControlBot : IBot
    {
        public string Process(dynamic input)
        {
            string t = input.text;
            if (t == "kebot pause")
            {
                KeBot.Run = false;
                return "Kebot paused";
            }
            if (t == "kebot resume")
            {
                KeBot.Run = true;
                return "Kebot resumed";
            }
            return null;
        }
    }
}
