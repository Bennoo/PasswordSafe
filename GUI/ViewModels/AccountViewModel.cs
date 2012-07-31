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

using Apex.MVVM;

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
