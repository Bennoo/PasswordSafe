using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Apex.MVVM;
//using PasswordSafe;

namespace GUI
{
    /// <summary>
    /// The AccountViewModel ViewModel class.
    /// </summary>
    [ViewModel]
    public class AccountViewModel : ViewModel
    {

        #region Properties
        /// <summary>
        /// The NotifyingProperty for the Name property.
        /// </summary>
        private readonly NotifyingProperty NameProperty =
          new NotifyingProperty("Name", typeof(string), default(string));

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        /// <value>The value of Name.</value>
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// The NotifyingProperty for the Encrypted property.
        /// </summary>
        private readonly NotifyingProperty EncryptedProperty =
          new NotifyingProperty("Encrypted", typeof(string), default(string));

        /// <summary>
        /// Gets or sets Encrypted.
        /// </summary>
        /// <value>The value of Encrypted.</value>
        public string Encrypted
        {
            get { return (string)GetValue(EncryptedProperty); }
            set { SetValue(EncryptedProperty, value); }
        }
        
        /// <summary>
        /// The NotifyingProperty for the Decrypted property.
        /// </summary>
        private readonly NotifyingProperty DecryptedProperty =
          new NotifyingProperty("Decrypted", typeof(string), default(string));

        /// <summary>
        /// Gets or sets Decrypted.
        /// </summary>
        /// <value>The value of Decrypted.</value>
        public string Decrypted
        {
            get { return (string)GetValue(DecryptedProperty); }
            set { SetValue(DecryptedProperty, value); }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountViewModel"/> class.
        /// </summary>
        public AccountViewModel()
        {
            
        }

        #region Commands

        /// <summary>
        /// Performs the GeneratePassword command.
        /// </summary>
        /// <param name="parameter">The GeneratePassword command parameter.</param>
        private void DoGeneratePasswordCommand(object parameter)
        {
            Encrypted = Crypto.Encrypt(Decrypted);
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



    }
}
