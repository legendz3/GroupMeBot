namespace KeBot.Service.Bots
{
    public class DongBot : IBot
    {
        public string Process(dynamic input)
        {
            if (input.text == "ding")
                return "dong";
            return null;
        }
    }
}
