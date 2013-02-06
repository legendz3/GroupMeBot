namespace KeBot.Service.Bots
{
    public class DongBot : IBot
    {
        public string Process(string input)
        {
            if (input == "ding")
                return "dong";
            return null;
        }
    }
}
