namespace KeBot.Service.Bots
{
    public class FuckDongBot : IBot
    {
        public string Process(string input)
        {
            if (input.ToLower().Contains("dongbot") && !input.ToLower().Contains("fuck dongbot"))
                return "fuck dongbot";
            return null;
        }
    }
}
