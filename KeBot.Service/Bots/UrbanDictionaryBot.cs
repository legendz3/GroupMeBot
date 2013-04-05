using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using RestSharp.Contrib;

namespace KeBot.Service.Bots
{
    public class UrbanDictionaryBot : IBot
    {
        public static string URL = "http://www.urbandictionary.com/define.php?term=";

        public static List<KeyValuePair<string, string>> Search(string query)
        {
            var ret = new List<KeyValuePair<string, string>>();

            string c = new WebClient().DownloadString(URL + HttpUtility.UrlEncode(query));
            MatchCollection mc = Regex.Matches(c, @"<div class=""definition"">(.*?)</div>");
            MatchCollection mc2 = Regex.Matches(c, @"<td class='word'>(.*?)</td>",
                RegexOptions.Singleline);
            if (mc.Count <= mc2.Count)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    ret.Add(new KeyValuePair<string, string>
                        (mc2[i].Groups[1].Value.Trim(),
                        HttpUtility.HtmlDecode(mc[i].Groups[1].Value.Trim())));
                }
            }

            return ret;
        }
        public string Process(dynamic input)
        {
            string t = input.text;
            if (t.ToLower().Contains(".ud"))
            {
                t = t.Remove(t.ToLower().IndexOf(".ud"), 4);
                var results = Search(t);
                if (results.Count > 0)
                {
                    var value = string.Concat(results[0].Key, " : ",  
                        results[0].Value.Count() > 400 ? results[0].Value.Substring(0, 400) + "..." : results[0].Value);
                    return value;
                }
            }
            return null;
        }
    }
}
