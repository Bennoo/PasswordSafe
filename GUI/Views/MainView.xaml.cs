﻿/******************************
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
            viewModel.GeneratePasswordCommand.Executed += new CommandEventHandler(GeneratePasswordCommand_Executed);
            viewModel.DeleteAccountCommand.Executing += new CancelCommandEventHandler(DeleteAccountCommand_Executing);
            viewModel.LoadAccountsCommand.Executing += new CancelCommandEventHandler(LoadAccountsCommand_Executing);
        }

        

        void GeneratePasswordCommand_Executed(object sender, CommandEventArgs args)
        {

        }

        void LoadAccountsCommand_Executing(object sender, CancelCommandEventArgs args)
        {
            //Properties.Settings.Default.MasterPassword = "";
            //Properties.Settings.Default.Save();

            // TODO: Figure out why the load passwords is not getting the master password from the property file correctly.

            // REMOVE THIS
            //return;

            string input = masterPassword.Text;
            if (input == "")
            {
                MessageBox.Show("Master password cannot be blank.", "Invalid master password.", MessageBoxButton.OK);
                args.Cancel = true;
            }
            else
            {
                //  The current master password
                string password = Properties.Settings.Default.MasterPassword;                

                //  Check to see if the settings master password is set
                if (password == "")
                {
                    Properties.Settings.Default.MasterPassword = Crypto.Encrypt(input);
                    Properties.Settings.Default.Save();
                }
                else
                {
                    string Decrypted = Crypto.Decrypt(password);
                    //  Compare to the stored MasterPassword.
                    if (input != Decrypted)
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
