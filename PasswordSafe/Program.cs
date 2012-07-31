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
using System.IO;
using System.Xml.Serialization;

namespace PasswordSafe
{
    class Program
    {
        public static string MasterPassword { get; set; }
        public static Passwords passwords { get; set; }
        public static string Helper { get; set; }
        public const string Shared = "sharedkey";

        static void Main(string[] args)
        {
            passwords = new Passwords();

            Console.WriteLine("-=-=-=-= Password Safe =-=-=-=-");
            if (!File.Exists("passwords.xml"))
            {
                Console.Write("Set master password: ");
                MasterPassword = Console.ReadLine();
                passwords.MasterPassword = Crypto.Encrypt(MasterPassword, Shared);
                Serialize();
                HandleUserInput();
            }
            else
            {
                Deserialize();
                Console.Write("Enter password: ");
                string input = Console.ReadLine();

                string currentPassword = Crypto.Decrypt(passwords.MasterPassword, Shared);
                
                if (currentPassword == input)
                {
                    MasterPassword = passwords.MasterPassword;
                    HandleUserInput();
                }
                else
                {
                    Console.WriteLine("Bad Password.  Press Any key.");
                    Console.ReadKey();
                }
            }
        }

        private static void HandleUserInput()
        {
            PrintMenu();
            string input = Console.ReadLine();
            while (input != "0")
            {
                switch (input)
                {
                    case "1":
                        Account account = new Account();
                        Console.Write("Account Name: ");
                        account.Name = Console.ReadLine();
                        Console.Write("Password: ");
                        account.Encrypted = Crypto.Encrypt(Console.ReadLine(), Shared);
                        passwords.Accounts.Add(account);
                        Serialize();
                        break;
                    case "2":
                        Console.WriteLine("");
                        foreach (Account a in passwords.Accounts)
                        {
                            Console.WriteLine("Account Name: {0}", a.Name);
                            Console.WriteLine("Password: {0}", Crypto.Decrypt(a.Encrypted, Shared));
                            Console.WriteLine("");
                        }
                        break;
                    case "3":
                        Console.WriteLine("");
                        Console.Write("This will regenerate all your passwords.  Do you wish to continue? (y/n)");
                        if (Console.ReadLine().ToUpper().Equals("Y") ||
                            Console.ReadLine().ToUpper().Equals("YES"))
                        {
                            string OriginalMasterPassword = MasterPassword;
                            Console.Write("New master password: ");
                            MasterPassword = Console.ReadLine();
                            passwords.MasterPassword = Crypto.Encrypt(MasterPassword, Shared);
                            //regenerate
                            foreach (Account a in passwords.Accounts)
                            {
                                Console.WriteLine("Account Name: {0}", a.Name);

                                //decrypt existing password
                                string decrypted = Crypto.Decrypt(a.Encrypted, Shared);
                                Console.WriteLine("Password: {0}", decrypted);
                                //encrypt using new master password                                
                                string encrypted = Crypto.Encrypt(decrypted, Shared);
                                a.Encrypted = encrypted;
                                Console.WriteLine("");
                            }
                            //save the new passwords
                            Serialize();

                        }

                        break;
                }

                PrintMenu();
                input = Console.ReadLine();
            }
        }

        private static void Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Passwords));
            TextWriter writer = new StreamWriter("passwords.xml");
            serializer.Serialize(writer, passwords);
            writer.Close();
        }

        private static void Deserialize()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Passwords));
            TextReader reader = new StreamReader("passwords.xml");
            Passwords p = (Passwords)deserializer.Deserialize(reader);
            reader.Close();
            passwords = p;
        }

        private static void PrintMenu()
        {
            Console.WriteLine("-=-=-=-= Password Safe =-=-=-=-");
            Console.WriteLine("1 - Add password");
            Console.WriteLine("2 - List passwords");
            Console.WriteLine("3 - Change master password");
            Console.WriteLine("0 - Exit");
            Console.Write("Enter selection: ");
        }

        
    }
}
