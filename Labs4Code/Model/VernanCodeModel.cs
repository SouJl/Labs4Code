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
    public class VernanCodeModel : PropertyChangedBase
    {

        private string _text;
        /// <summary>
        /// Кодируемый текст.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    NotifyOfPropertyChange(() => Text);
                }
            }
        }

        private string _key;
        /// <summary>
        /// Ключ шифрования (из файла)
        /// </summary>
        public string Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;
                    NotifyOfPropertyChange(() => Key);
                }
            }
        }


        public VernanCodeModel() { }

        /// <summary>
        /// Команда генерацим ключа
        /// </summary>
        public ICommand RandKeyCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    if (Text != null)
                        Key = RandKey(Text);
                    else
                        MessageBox.Show("Введите кодируемый текст", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                });
            }
        }

        /// <summary>
        /// Команда кодирования информации
        /// </summary>
        public ICommand EncrypteCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Text = Coding(Text.ToCharArray(), Key.ToCharArray());
                });
            }
        }

        /// <summary>
        /// Команда декодирования информации
        /// </summary>
        public ICommand DecrypteCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Text = Coding(Text.ToCharArray(), Key.ToCharArray());
                });
            }
        }

        /// <summary>
        /// Команда записи/чтения ключа из файла
        /// </summary>
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


        private string RandKey(string str)
        {
            string result = null;
            Random rand = new Random();
            for (int i = 0; i < str.Length; i++)
            {
                result += (char)(rand.Next(65, 122));
            }
            return result;
        }


        private string Coding(char[] str, char[] key)
        {
            string result = null;
            for (int i = 0; i < str.Length; i++)
            {
                result += (char)(str[i] ^ key[i]);
            }
            return result.ToString();
        }

    }
}
