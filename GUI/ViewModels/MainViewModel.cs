/******************************
 * Copyright (C) 2012  Ryan Perneel

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ******************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Apex.MVVM;

namespace GUI
{
    /// <summary>
    /// The MainViewModel ViewModel class.
    /// </summary>
    [ViewModel]
    public class MainViewModel : ViewModel
    {

        public Data data { get; set; }
        public String Shared { get; set; }


        #region Properties
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
        private ObservableCollection<Account> AccountsProperty =
          new ObservableCollection<Account>();

        /// <summary>
        /// Gets the Accounts observable collection.
        /// </summary>
        /// <value>The Accounts observable collection.</value>
        public ObservableCollection<Account> Accounts
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
        public Account SelectedAccount
        {
            get { return (Account)GetValue(SelectedAccountProperty); }
            set
            {
                SetValue(SelectedAccountProperty, value);
                DeleteAccountCommand.CanExecute = value != null;
                GeneratePasswordCommand.CanExecute = value != null; 
            }
        } 
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            LoadAccountsCommand = new Command(DoLoadAccountsCommand);
            AddAccountCommand = new Command(DoAddAccountCommand, false);
            DeleteAccountCommand = new Command(DoDeleteAccountCommand, false);
            GeneratePasswordCommand = new Command(DoGeneratePasswordCommand, false);
            ResetPasswordCommand = new Command(DoResetPasswordCommand, true);
        }

        #region Commands

        /// <summary>
        /// Performs the LoadAccounts command.
        /// </summary>
        /// <param name="parameter">The LoadAccounts command parameter.</param>
        private void DoLoadAccountsCommand(object parameter)
        {
            Deserialize();
            AddAccountCommand.CanExecute = true;
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
            //  make the view visible

            //  Create an account
            Account newAccount = new Account()
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

        /// <summary>
        /// Performs the GeneratePassword command.
        /// </summary>
        /// <param name="parameter">The GeneratePassword command parameter.</param>
        private void DoGeneratePasswordCommand(object parameter)
        {
            SelectedAccount.Encrypted = Crypto.Encrypt(SelectedAccount.Decrypted);
            
            Account temp = SelectedAccount;

            SelectedAccount = null;

            SelectedAccount = temp;

            temp = null;

            Serialize();
        }



        /// <summary>
        /// Performs the ResetPassword command.
        /// </summary>
        /// <param name="parameter">The ResetPassword command parameter.</param>
        private void DoResetPasswordCommand(object parameter)
        {
            ResetPasswordView view = new ResetPasswordView();
            view.ShowDialog();
        }

        /// <summary>
        /// Gets the ResetPassword command.
        /// </summary>
        /// <value>The value of .</value>
        public Command ResetPasswordCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the GeneratePassword command.
        /// </summary>
        /// <value>The value of .</value>
        public Command GeneratePasswordCommand
        {
            get;
            private set;
        }

        #endregion

        #region Helpers
        public void Serialize()
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Account>));
            using (StreamWriter wr = new StreamWriter("accounts.xml"))
            {
                xs.Serialize(wr, Accounts);
            }
        }

        public void Deserialize()
        {
            List<Account> temp;

            XmlSerializer xs = new XmlSerializer(typeof(List<Account>));
            using (StreamReader rd = new StreamReader("accounts.xml"))
            {
                temp = xs.Deserialize(rd) as List<Account>;
            }

            //add to the collection
            foreach (var account in temp)
            {
                Accounts.Add(new Account()
                {
                    Name = account.Name,
                    Encrypted = account.Encrypted,
                    Decrypted = Crypto.Decrypt(account.Encrypted),
                    Type = account.Type
                });
            }
        }
        #endregion
    }
}
