namespace KeBot.Service.Bots
{
    public class KbbqBud : IBot
    {
        public string Process(dynamic input)
        {
            string t = input;
            string s = "Keyword kbbq detected. Here is the reservation link to Yakiniq  http://talkto.com/tt/100da026/?u1=xc6aaux";
            if (t != s && t.ToLower().Contains("kbbq"))
                return s;
            return null;
        }
    }
}
