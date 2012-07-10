using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Apex.MVVM;

namespace GUI
{
    /// <summary>
    /// Interaction logic for AccountView.xaml
    /// </summary>
    [View(typeof(AccountViewModel))]
    public partial class AccountView : UserControl
    {
        public AccountView()
        {
            InitializeComponent();
        }

        public void FocusAccountName()
        {
            accountName.Focus();
        }
    }
}
