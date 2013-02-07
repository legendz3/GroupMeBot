namespace KeBot.Service.Bots
{
    public class KbbqBud : IBot
    {
        public string Process(string input)
        {
            string s = "Keyword kbbq detected. Here is the reservation link to Yakiniq  http://talkto.com/tt/100da026/?u1=xc6aaux";
            if (input != s && input.ToLower().Contains("kbbq"))
                return s;
            return null;
        }
    }
}
