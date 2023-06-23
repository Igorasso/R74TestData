using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealestaData
{
    internal class RealestaDeathsModel
    {
        public RealestaDeathsModel(string Date, string Killers)
        {
            this.Date = Date;
            this.Killers = Killers;


        }

        public string Date { get; set; }
        public string Killers { get; set; }
    }
}
