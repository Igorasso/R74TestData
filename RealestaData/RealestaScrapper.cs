using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;


namespace RealestaData
{
    internal class RealestaScrapper
    {
        private const string GetPlayersUrl = "https://realesta74-net.translate.goog/highscores/renoria/experience/all-vocations?_x_tr_sl=auto&_x_tr_tl=pl&_x_tr_hl=eng&_x_tr_pto=wapp";
        private const string GetDeathsUrl = "https://realesta74-net.translate.goog/latest-deaths?_x_tr_sl=auto&_x_tr_tl=pl&_x_tr_hl=eng&_x_tr_pto=wapp";

        private const string GetPLayersUrlWithoutTransalte =
            "https://realesta74.net/highscores/renoria/experience/all-vocations";
        public List<RealestaPlayerModel> GetPlayers()
        {
            List<RealestaPlayerModel> realestaPlayerModelsList = new List<RealestaPlayerModel>();
            var web = new HtmlWeb();
            //var document = web.Load(GetPlayersUrl);
            var document = web.Load(GetPLayersUrlWithoutTransalte);
            //skipping the first row in the table what cointaint type attributes
            //taking first 15 rows from the table
           // var finalTableRows = new HtmlWeb().Load(GetPlayersUrl).QuerySelectorAll("table")[1].QuerySelectorAll("tr").Skip(1).Take(15);
            var finalTableRows = new HtmlWeb().Load(GetPLayersUrlWithoutTransalte).QuerySelectorAll("table")[1].QuerySelectorAll("tr").Skip(1).Take(15);
            List<string> urls = new List<string>();

            foreach (var tableRow in finalTableRows)
            {
                var tds = tableRow.QuerySelectorAll("td");
                var Id = tds[0].InnerText;
                var Name = tds[1].InnerText; //inner html for ahref tags
                string href = null;

                if (Name == "Name")
                { }
                else
                    href = tds[1].QuerySelector("a").Attributes["href"].Value;

                var temp = tds[1];
                var Vocation = tds[2].InnerText;
                var PlayersLevel = tds[3].InnerText;
                var Experience = tds[4].InnerText;
                if (href != null)
                {
                    urls.Add(href);
                }
                realestaPlayerModelsList.Add(new RealestaPlayerModel(Id, Name, Vocation, PlayersLevel, Experience, href));
            }
            return realestaPlayerModelsList;
        }
        public List<RealestaDeathsModel> GetDeaths()
        {
            List<RealestaDeathsModel> realestaDeathsList = new List<RealestaDeathsModel>();
            var finalTableRows = new HtmlWeb().Load(GetDeathsUrl).QuerySelectorAll("table").QuerySelectorAll("tr")
                .Skip(1).Take(15);
            foreach (var tableRow in finalTableRows)
            {
                var tds = tableRow.QuerySelectorAll("td");
                var Date = tds[0].InnerText;
                var Killers = tds[1].InnerText;


                realestaDeathsList.Add(new RealestaDeathsModel(Date, Killers));
            }

            return realestaDeathsList;
        }
        public string GetStatus(string Url)
        {
            //Thread.Sleep(500);
            var page = new HtmlWeb().Load("https://realesta74.net/"+Url).QuerySelectorAll("table");
            if(page != null)
            {foreach (var row in page)
            {
                if (row.InnerText.Contains("Online"))
                    return "online";
            }
                return "offline";
            }

            return "unknown";




        }
        public string GetStatusTest(string Url)
        {
            string result;
            Random random = new Random();
            int i = random.Next(0, 4);
            if (i == 1)
            {
                result = "online";
            }
            else if (i == 2)
            {
                result = "offline";
            }
            else
            {
                result = "unknown";
            }
            return result;
        }
    }
}