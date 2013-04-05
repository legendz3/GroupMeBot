namespace KeBot.Service.Bots
{
    public class HotpotBud : IBot
    {
        public string Process(dynamic input)
        {
            string t = input.text;
            string s = "Keyword hotpot detected. Here is the reservation link to The Pot's http://talkto.com/tt/7f435f8b/?u1=xc6aaux";
            if (t != s && (t.ToLower().Contains("hotpot") || t.ToLower().Contains("hot pot")))
                return s;
            return null;
        }
    }
}
