namespace KeBot.Service.Bots
{
    public class HotpotBud : IBot
    {
        public string Process(string input)
        {
            
            string s = "Keyword hotpot detected. here is the reservation link to The Pot's http://talkto.com/tt/7f435f8b/?u1=xc6aaux";
            if (input!= s && (input.ToLower().Contains("hotpot") || input.ToLower().Contains("hot pot")))
                return s;
            return null;
        }
    }
}
