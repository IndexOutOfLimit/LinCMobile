using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cognizant.Hackathon.Mobile.Core.Models;
using Cognizant.Hackathon.Shared.Mobile.Models.Enums;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    [Serializable]
    public class LinCControl : ModelBase, INotifyPropertyChanged
    {
        List<LinCComonModel> _controlValues;
        int _selectedIndex;

        public bool IsVisible { get; set; }
        public string Title { get; set; }
        public LinCComonModel DefaultItem { get; set; }
        public List<LinCComonModel> ControlValues
        {
            get
            {
                return _controlValues;
            }
            set
            {
                if(value != null)
                {
                    _controlValues = value;
                    RaisePropertyChanged("ControlValues");
                }
            }
        }
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if(value > -1)
                {
                    _selectedIndex = value;
                    RaisePropertyChanged("SelectedIndex");
                }                
            }
        }
        public string ParentId { get; set; }

        public LinCControl ShallowCopy()
        {
            return (LinCControl)this.MemberwiseClone();
        }
    }

    public class LinCComonModel : ModelBase
    {
        private LinCComonModel item;

        public LinCComonModel(LinCComonModel item)
        {
            this.item = item;
        }

        public LinCComonModel()
        {
        }

        public string Name { get; set; }
        public string Value { get; set; }        
        public bool IsNew { get; set; }
        public string ItemId { get; set; }
    }
}
