using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GUI
{
    [Serializable, XmlRoot("Accounts")]
    public class Account
    {
        [XmlElement]
        public String Name { get; set; }
        [XmlElement]
        public String Encrypted { get; set; }
        [XmlIgnore]
        public String Decrypted { get; set; }
    }
}
