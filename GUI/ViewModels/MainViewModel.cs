using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Apex.MVVM;
//using PasswordSafe;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;

namespace GUI
{
    /// <summary>
    /// The MainViewModel ViewModel class.
    /// </summary>
    [ViewModel]
    public class MainViewModel : ViewModel
    {

        public Data data { get; set; }
        //public String MasterPassword { get; set; }
        public String Shared { get; set; }

        
        /// <summary>
        /// The NotifyingProperty for the MasterPassword property.
        /// </summary>
        private readonly NotifyingProperty MasterPasswordProperty =
          new NotifyingProperty("MasterPassword", typeof(String), default(String));

        /// <summary>
        /// Gets or sets MasterPassword.
        /// </summary>
        /// <value>The value of MasterPassword.</value>
        public String MasterPassword
        {
            get { return (String)GetValue(MasterPasswordProperty); }
            set { SetValue(MasterPasswordProperty, value); }
        }
   
        /// <summary>
        /// The Accounts observable collection.
        /// </summary>
        private ObservableCollection<AccountViewModel> AccountsProperty =
          new ObservableCollection<AccountViewModel>();

        /// <summary>
        /// Gets the Accounts observable collection.
        /// </summary>
        /// <value>The Accounts observable collection.</value>
        public ObservableCollection<AccountViewModel> Accounts
        {
            get { return AccountsProperty; }
        }
        
        /// <summary>
        /// The NotifyingProperty for the SelectedAccount property.
        /// </summary>
        private readonly NotifyingProperty SelectedAccountProperty =
          new NotifyingProperty("SelectedAccount", typeof(AccountViewModel), default(AccountViewModel));

        /// <summary>
        /// Gets or sets SelectedAccount.
        /// </summary>
        /// <value>The value of SelectedAccount.</value>
        public AccountViewModel SelectedAccount
        {
            get { return (AccountViewModel)GetValue(SelectedAccountProperty); }
            set { SetValue(SelectedAccountProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            MasterPassword = ConfigurationSettings.AppSettings.Get("MasterPassword");
            data = new Data();
            //Deserialize();
            Accounts.Add(new AccountViewModel() { Name = "test", Encrypted = "1234", Decrypted = "3214" });
            Accounts.Add(new AccountViewModel() { Name = "test2", Encrypted = "aklsdgf", Decrypted = "alskdgfa" });

            data.Accounts = Accounts;

        }

        private void Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Data));
            TextWriter writer = new StreamWriter("passwords.xml");
            serializer.Serialize(writer, data);
            writer.Close();
        }
        /*
        private void Deserialize()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Passwords));
            TextReader reader = new StreamReader("passwords.xml");
            Passwords p = (Passwords)deserializer.Deserialize(reader);
            reader.Close();
            passwords = p;
        }*/

        
    }
}
