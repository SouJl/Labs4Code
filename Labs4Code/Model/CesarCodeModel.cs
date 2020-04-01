using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Labs4Code.Static;
using Microsoft.Win32;

namespace Labs4Code.Model
{
    class CesarCodeModel: PropertyChangedBase
    {
        #region Алфавиты

        private const string RuAlfabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        private const string EnAlfabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string FullRuAlfabet { get; set; }

        private string FullEnAlfabet { get; set; }

        private string CurrentAlfabet { get; set; }

        #endregion


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

        private int key;
        /// <summary>
        /// Ключ кодирования
        /// </summary>
        public int Key
        {
            get => key;
            set
            {
                if (key != value)
                {
                    key = value;
                    NotifyOfPropertyChange(() => Key);
                }
            }
        }



        private string _filepath;
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FilePath
        {
            get => _filepath;
            set
            {
                if (_filepath != value)
                {
                    _filepath = value;
                    NotifyOfPropertyChange(() => FilePath);
                }
            }
        }

        /// <summary>
        /// Структура расшифрованых последовательностей
        /// </summary>
        public struct DecryptElement
        {
            public int Index { get; set; }
            public string Element { get; set; }
        }

        private ObservableCollection<DecryptElement> decryptetexts;
        /// <summary>
        /// Коллекция расшифрованых последовательностей
        /// </summary>
        public ObservableCollection<DecryptElement> DecrypteTexts
        {
            get => decryptetexts;
            set
            {
                decryptetexts = value;
                NotifyOfPropertyChange(() => DecrypteTexts);
            }
        }

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public CesarCodeModel()
        {
            FullRuAlfabet = RuAlfabet + RuAlfabet.ToLower();
            FullEnAlfabet = EnAlfabet + EnAlfabet.ToLower();
        }

        #region Команды

        /// <summary>
        /// Команда получения результата кодирования
        /// </summary>
        public ICommand GetResultCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    var CommandParametr = (string)sender;
                    CurrentAlfabet = GetAlfabet(TextIn[0]);
                    /* int[] mass = new int[TextIn.Length];
                     for (int i = 0; i < mass.Length; i++)
                         mass[i] = (int)TextIn[i];*/
                    switch (CommandParametr)
                    {
                        case "Encrypt":
                            {
                                TextOut = GetEncode(TextIn, Key);
                                break;
                            }
                        case "Decrypt":
                            {
                                TextOut = GetEncode(TextIn, -Key);
                                break;
                            }
                    }
                });
            }
        }

        /// <summary>
        /// Команда криптоанализа последовательности
        /// </summary>
        public ICommand GetAllDecrypteCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    DecrypteTexts = new ObservableCollection<DecryptElement>();
                    CurrentAlfabet = GetAlfabet(TextIn[0]);
                    for (int i = 1; i < CurrentAlfabet.Length; i++)
                    {
                        DecrypteTexts.Add(new DecryptElement
                        {
                            Index = i,
                            Element = GetEncode(TextOut, -i)
                        });
                    }
                });
            }
        }

        /// <summary>
        /// Команда откытия файла
        /// </summary>
        public ICommand OpenFileCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    var file = new OpenFileDialog();
                    file.Filter = "text files (*.txt)|*.txt";
                    if (file.ShowDialog() == true)
                    {
                        FilePath = file.FileName;
                    }
                });
            }
        }

        /// <summary>
        /// Команда шифрования информации в файле
        /// </summary>
        public ICommand EncrypteFileCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    FileStream writeFile = new FileStream("Encrypte.txt", FileMode.OpenOrCreate);
                    StreamReader read_stream = new StreamReader(FilePath);
                    StreamWriter write_stream = new StreamWriter(writeFile);
                    while (read_stream.Peek() > 0)
                    {
                        byte symbol = (byte)read_stream.Read();
                        symbol += (byte)Key;
                        write_stream.Write(Convert.ToChar(symbol));
                    }
                    read_stream.Close();
                    write_stream.Close();
                });
            }
        }

        /// <summary>
        /// Команда дешифрования информации в файле
        /// </summary>
        public ICommand DecrypteFileCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    FileStream writeFile = new FileStream("Decrypte.txt", FileMode.OpenOrCreate);
                    StreamReader read_stream = new StreamReader("Encrypte.txt");
                    StreamWriter write_stream = new StreamWriter(writeFile);
                    while (read_stream.Peek() > 0)
                    {
                        byte symbol = (byte)read_stream.Read();
                        symbol -= (byte)Key;
                        write_stream.Write(Convert.ToChar(symbol));
                    }
                    read_stream.Close();
                    write_stream.Close();
                });
            }
        }

        #endregion

        #region Методы

        private string GetAlfabet(char symbol)
        {
            string result = null;
            if (FullRuAlfabet.Contains(symbol))
            {
                result = FullRuAlfabet;
            }
            if (FullEnAlfabet.Contains(symbol))
            {
                result = FullEnAlfabet;
            }
            return result;
        }

        private string GetEncode(string text, int k)
        {
            string result = null;
            for (int i = 0; i < text.Length; i++)
            {
                int index = CurrentAlfabet.IndexOf(text[i]);
                if (index < 0)
                    result += text[i];
                else
                    result += CurrentAlfabet[(CurrentAlfabet.Length + index + k) % CurrentAlfabet.Length];
            }
            return result;
        }

        #endregion
    }
}
