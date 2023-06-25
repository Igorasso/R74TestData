using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RealestaData
{
    internal class RealestaPlayerModel : INotifyPropertyChanged
    {
        private string _status = string.Empty;

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
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

