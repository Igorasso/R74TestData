namespace RealestaData
{
    internal class RealestaPlayerModel
    {
        private string _status = string.Empty;
        static private int _fakestatus = 0;

        public RealestaPlayerModel(string Id, string Name, string Vocation, string PlayersLevel, string Experience, string Href)
        {
            this.Id = Id;
            this.Name = Name;
            this.Vocation = Vocation;
            this.PlayersLevel = PlayersLevel;
            this.Experience = Experience;
            this.Href = Href;

            //use it to test IpropertyChanged interface
            //_fakestatus++;
            //_status = _fakestatus % 2 == 0 ? "online" : "offline";

        }
        public string Id { get; set; }
        public string Name { get; set; }

        public string Vocation { get; set; }
        public string PlayersLevel { get; set; }
        public string Experience { get; set; }
        public string Href { get; set; }

        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                }
            }
        }
    }
}

