using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealestaData
{
    internal class RealestaPlayerModel
    {
        public RealestaPlayerModel(string Id, string Name, string Vocation, string PlayersLevel, string Experience, string Href)
        {
            this.Id = Id;
            this.Name = Name;
            this.Vocation = Vocation;
            this.PlayersLevel = PlayersLevel;
            this.Experience = Experience;
            this.Href = Href;

        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Vocation { get; set; }
        public string PlayersLevel { get; set; }
        public string Experience { get; set; }
        public string Href { get; set; }
    }
}

