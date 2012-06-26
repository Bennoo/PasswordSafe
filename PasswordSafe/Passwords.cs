using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PasswordSafe
{
    [Serializable, XmlRoot("Passwords")]
    public class Passwords
    {
        [XmlElement]
        public String MasterPassword { get; set; }
        [XmlElement]
        public List<Account> Accounts { get; set; }
        
        public Passwords()
        {
            this.Accounts = new List<Account>();
        }
    }
}
