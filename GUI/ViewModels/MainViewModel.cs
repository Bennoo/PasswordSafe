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
using System.ComponentModel;

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
            set 
            { 
                SetValue(MasterPasswordProperty, value);
                Console.WriteLine("MasterPassword: {0}", MasterPassword);
            }
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
            set 
            { 
                SetValue(SelectedAccountProperty, value);
                DeleteAccountCommand.CanExecute = value != null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            /*MasterPassword = ConfigurationSettings.AppSettings.Get("MasterPassword");
            data = new Data();
            //Deserialize();
            Accounts.Add(new AccountViewModel() { Name = "test", Encrypted = "1234", Decrypted = "3214" });
            Accounts.Add(new AccountViewModel() { Name = "test2", Encrypted = "aklsdgf", Decrypted = "alskdgfa" });

            data.Accounts = Accounts;*/

            LoadAccountsCommand = new Command(DoLoadAccountsCommand);
            AddAccountCommand = new Command(DoAddAccountCommand, false);
            DeleteAccountCommand = new Command(DoDeleteAccountCommand, false);

        }

        #region Commands

        /// <summary>
        /// Performs the LoadAccounts command.
        /// </summary>
        /// <param name="parameter">The LoadAccounts command parameter.</param>
        private void DoLoadAccountsCommand(object parameter)
        {
            AddAccountCommand.CanExecute = true;
            //deserialize

        }

        /// <summary>
        /// Gets the LoadCommand command.
        /// </summary>
        /// <value>The value of .</value>
        public Command LoadAccountsCommand
        {
            get;
            private set;
        }



        /// <summary>
        /// Performs the AddAccount command.
        /// </summary>
        /// <param name="parameter">The AddAccount command parameter.</param>
        private void DoAddAccountCommand(object parameter)
        {
            //  Create an account
            AccountViewModel newAccount = new AccountViewModel()
            {
                Name = "New Account"
            };

            //  Add to the list
            Accounts.Add(newAccount);

            //Select the account.
            SelectedAccount = newAccount;
        }

        /// <summary>
        /// Gets the AddAccount command.
        /// </summary>
        /// <value>The value of .</value>
        public Command AddAccountCommand
        {
            get;
            private set;
        }



        /// <summary>
        /// Performs the DeleteAccount command.
        /// </summary>
        /// <param name="parameter">The DeleteAccount command parameter.</param>
        private void DoDeleteAccountCommand(object parameter)
        {
            //  Remove the account from the list
            Accounts.Remove(SelectedAccount);

            //  Clear the SelectedAccount
            SelectedAccount = null;
        }

        /// <summary>
        /// Gets the DeleteAccount command.
        /// </summary>
        /// <value>The value of .</value>
        public Command DeleteAccountCommand
        {
            get;
            private set;
        }

        #endregion

        private void Serialize<T>(T item)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter("passwords.xml"))
            {
                serializer.Serialize(writer, item);
            }
        }
        
        private void Deserialize()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Data));
            TextReader reader = new StreamReader("passwords.xml");
            Data p = (Data)deserializer.Deserialize(reader);
            reader.Close();
        }       
    }
}
