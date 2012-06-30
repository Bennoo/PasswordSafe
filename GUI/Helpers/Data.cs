using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.ObjectModel;

namespace GUI
{
    [Serializable, XmlRoot("Data")]
    public class Data
    {        
        [XmlElement]
        public String MasterPassword { get; set; }
        [XmlElement]
        public ObservableCollection<AccountViewModel> Accounts { get; set; }

        public Data()
        {
            this.Accounts = new ObservableCollection<AccountViewModel>();
        }
    }
}
