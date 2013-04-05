namespace KeBot.Service.Bots
{
    public class FuckDongBot : IBot
    {
        public string Process(dynamic input)
        {
            string t = input.text;
            if (t.ToLower().Contains("dongbot") && !t.ToLower().Contains("fuck dongbot"))
                return "fuck dongbot";
            return null;
        }
    }
}
