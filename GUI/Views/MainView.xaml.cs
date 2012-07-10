using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Apex.MVVM;
using System.Windows;
using System.Configuration;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    [View(typeof(MainViewModel))]
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            viewModel.AddAccountCommand.Executed += new CommandEventHandler(AddAccountCommand_Executed);
            viewModel.DeleteAccountCommand.Executing += new CancelCommandEventHandler(DeleteAccountCommand_Executing);
            viewModel.LoadAccountsCommand.Executing += new CancelCommandEventHandler(LoadAccountsCommand_Executing);
        }

        void LoadAccountsCommand_Executing(object sender, CancelCommandEventArgs args)
        {
            string input = masterPassword.Text;
            if (input == "")
            {
                MessageBox.Show("Master password cannot be blank.", "Invalid master password.", MessageBoxButton.OK);
                args.Cancel = true;
            }
            else
            {
                //  Encrypt the input
                string Encrypted = Crypto.Encrypt(input);

                //  The current master password
                string password = Properties.Settings.Default.MasterPassword;

                //  Check to see if the settings master password is set
                if (password == "")
                {
                    Properties.Settings.Default.MasterPassword = Encrypted;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    //  Compare to the stored MasterPassword.
                    if (password != Encrypted)
                    {
                        MessageBox.Show("Incorrect master password entered.", "Invalid master password", MessageBoxButton.OK);
                        args.Cancel = true;
                    }
                }
            }
        }

        void DeleteAccountCommand_Executing(object sender, CancelCommandEventArgs args)
        {
            args.Cancel = MessageBox.Show("Are you sure?", "Delete Account", MessageBoxButton.YesNo) != MessageBoxResult.Yes;
        }

        void AddAccountCommand_Executed(object sender, CommandEventArgs args)
        {
            accountView.FocusAccountName();
        }
    }
}
