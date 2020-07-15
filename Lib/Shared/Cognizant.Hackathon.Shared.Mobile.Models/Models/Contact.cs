using System;
using Cognizant.Hackathon.Mobile.Core.Models;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    public class Contact : ModelBase
    {
        public string Name { get; set; }
        public string[] Emails { get; set; }
        //public string[] PhoneNumbers { get; set; }
        private bool _isSelected;

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
    }
}
