using System;
using System.ComponentModel;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    public class ScrollableHeaderItemModel : INotifyPropertyChanged

    {
        public string Header { get; set; }
        public bool IsSelected { get; set; }
        public int Index { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
