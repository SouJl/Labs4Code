using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Labs4Code.Static;

namespace Labs4Code.Model
{
    class VernanCodeModel : PropertyChangedBase
    {
        private const string RuAlfabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        private const string EnAlfabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private List<string> MainAlph { get; set; }

        private string textin;
        /// <summary>
        /// Кодируемый текс
        /// </summary>
        public string TextIn
        {
            get => textin;
            set
            {
                if (textin != value)
                {
                    textin = value;
                    NotifyOfPropertyChange(() => TextIn);
                }
            }
        }

        private string textout;
        /// <summary>
        /// Закодированный текст
        /// </summary>
        public string TextOut
        {
            get => textout;
            set
            {
                if (textout != value)
                {
                    textout = value;
                    NotifyOfPropertyChange(() => TextOut);
                }
            }
        }

        /// <summary>
        /// Ключ шифрования (из файла)
        /// </summary>
        private string Key { get; set; }


        public VernanCodeModel()
        {

        }


        public ICommand RandKeyCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    if (TextIn != null)
                        Key = RandKey(TextIn);
                    else
                        MessageBox.Show("Введите кодируемый текст", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                });
            }
        }
        public ICommand EncrypteCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    TextOut = Coding(TextIn.ToCharArray(), Key.ToCharArray());
                });
            }
        }

        public ICommand DecrypteCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Coding(TextOut.ToCharArray(), Key.ToCharArray());
                });
            }
        }

        public ICommand KeyFileCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    string parametr = (string)sender;
                    switch (parametr)
                    {
                        case "Read":
                            {
                                using (FileStream fstream = File.OpenRead($"Key.txt"))
                                {
                                    byte[] array = new byte[fstream.Length];
                                    fstream.Read(array, 0, array.Length);
                                    Key = Encoding.Default.GetString(array);
                                }
                                break;
                            }
                        case "Write":
                            {
                                using (FileStream fstream = new FileStream($"Key.txt", FileMode.OpenOrCreate))
                                {
                                    byte[] array = Encoding.Default.GetBytes(Key);
                                    fstream.Write(array, 0, array.Length);
                                    MessageBox.Show("Ключ записан в файл");
                                }
                                break;
                            }
                    }
                });
            }
        }

        static string RandKey(string str)
        {
            char[] key = new char[str.Length];
            Random rand = new Random();
            for (int i = 0; i < str.Length; i++)
            {
                key[i] = (char)(rand.Next() % 255);
            }
            return key.ToString();
        }


        private string Coding(char[] str, char[] key)
        {
            string result = null;
            for (int i = 0; i < str.Length; i++)
            {
                result += ((char)(str[i] ^ key[i]));
            }
            return result.ToString();
        }

    }
}
