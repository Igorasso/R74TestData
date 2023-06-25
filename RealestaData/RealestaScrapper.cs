using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace RealestaData
{
    internal class RealestaScrapper
    {
        private const string GetPlayersUrl = "https://realesta74-net.translate.goog/highscores/renoria/experience/all-vocations?_x_tr_sl=auto&_x_tr_tl=pl&_x_tr_hl=eng&_x_tr_pto=wapp";
        private const string GetDeathsUrl = "https://realesta74-net.translate.goog/latest-deaths?_x_tr_sl=auto&_x_tr_tl=pl&_x_tr_hl=eng&_x_tr_pto=wapp";

        public List<RealestaPlayerModel> GetPlayers() 
        {
            List<RealestaPlayerModel> realestaPlayerModelsList = new List<RealestaPlayerModel>();
            var web = new HtmlWeb();
            var document = web.Load(GetPlayersUrl);
            //skipping the first row in the table what cointaint type attributes
            //taking first 15 rows from the table
            var finalTableRows = new HtmlWeb().Load(GetPlayersUrl).QuerySelectorAll("table")[1].QuerySelectorAll("tr").Skip(1).Take(15);
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

            //foreach (var url in urls)
            //{
            //    Thread.Sleep(500);
            //    document = web.Load(url);



            //}



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
            var page = new HtmlWeb().Load(GetDeathsUrl).QuerySelectorAll("table");

            string result = page.Any(row => row.InnerText.Contains("online")) != null
                ? "online"
                : page.Any(row => row.InnerText.Contains("offline")) != null
                    ? "offline"
                    : "unknown";
            return result;
        }
    }
}