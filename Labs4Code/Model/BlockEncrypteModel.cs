using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Labs4Code.Static;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace Labs4Code.Model
{
    public class BlockEncrypteModel : PropertyChangedBase
    {

        private bool _visibility;
        /// <summary>
        /// Отображения параметров ключа.
        /// </summary>
        public bool Visibility
        {
            get => _visibility;
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    NotifyOfPropertyChange(() => Visibility);
                }
            }
        }
        private bool VisStatusChange { get; set; }

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

        private List<byte> Key { get; set; }

        private DES DESshifr { get; set; }

        private List<byte> data { get; set; }

        public BlockEncrypteModel()
        {
            Key = new List<byte>();
            data = new List<byte>();
            DESshifr = DES.Create();
        }

        public ICommand KeyOpenParamCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    switch (VisStatusChange)
                    {
                        case true:
                            {
                                Visibility = false;
                                VisStatusChange = false;
                                break;
                            }
                        case false:
                            {
                                Visibility = true;
                                VisStatusChange = true;
                                break;
                            }
                    }
                });
            }
        }

        public ICommand EncrypteCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    data.Clear();
                    data.AddRange(Encrypte(Text, Key.ToArray(), DESshifr.IV));
                    Text = Encoding.Default.GetString(data.ToArray());
                });
            }
        }

        public ICommand DecrypteCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Text = Decrypte(Encoding.Default.GetBytes(Text), Key.ToArray(), DESshifr.IV);
                });
            }
        }

        public ICommand RandKeyCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    DESshifr.GenerateKey();
                    Key.Clear();
                    Key.AddRange(DESshifr.Key);
                });
            }
        }

        /// <summary>
        /// Команда загрузки ключа
        /// </summary>
        public ICommand LoadKeyCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    var file = new OpenFileDialog();
                    string filepath = null;
                    file.Filter = "text files (*.txt)|*.txt";
                    if (file.ShowDialog() == true)
                    {
                        filepath = file.FileName;
                    }
                    Key.Clear();
                    Key.AddRange(ReadKeyFromFile(filepath));
                });
            }
        }


        /// <summary>
        /// Команда записи ключа в файл
        /// </summary>
        public ICommand SaveKeyCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    var file = new OpenFileDialog();
                    string filepath = null;
                    file.Filter = "text files (*.txt)|*.txt";
                    if (file.ShowDialog() == true)
                    {
                        filepath = file.FileName;
                    }
                    SaveKeyToFile(filepath, Key);
                });
            }
        }

        private byte[] ReadKeyFromFile(string filepath)
        {
            List<byte> key = new List<byte>();
            using (StreamReader read_stream = new StreamReader(filepath))
            {
                while (read_stream.Peek() > 0)
                {
                    byte symbol = (byte)read_stream.Read();
                    key.Add(symbol);
                }
                read_stream.Close();
            }
            return key.ToArray();
        }

        private void SaveKeyToFile(string filepath, List<byte> key)
        {
            FileStream writeFile = new FileStream(filepath, FileMode.OpenOrCreate);
            using (StreamWriter write_stream = new StreamWriter(writeFile))
            {
                for(int i =0; i < key.Count; i++)
                {
                    byte symbol = key[i];
                    write_stream.Write(Convert.ToChar(symbol));
                }
                write_stream.Close();
            }
        }

        public string DESCryptOperation(string data, byte[] Key, byte[] IV, bool ecnrypte)
        {
            DESshifr.Key = Key;
            DESshifr.IV = IV;
            byte[] inBlock = Encoding.Unicode.GetBytes(data);
            ICryptoTransform xfrm = null;
            if (ecnrypte)
            {
                xfrm = DESshifr.CreateEncryptor();
            }
            if (!ecnrypte)
            {
                xfrm = DESshifr.CreateDecryptor();
            }
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, 0, inBlock.Length);
            return Encoding.Unicode.GetString(outBlock);
        }

        public byte[] Encrypte(string data, byte[] Key, byte[] IV)
        {
            DESshifr.Key = Key;
            DESshifr.IV = IV;
            byte[] inBlock = Encoding.Default.GetBytes(data);
            ICryptoTransform xfrm = DESshifr.CreateEncryptor();
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, 0, inBlock.Length);
            return outBlock;
        }
        public string Decrypte(byte[] data, byte[] Key, byte[] IV)
        {
            try
            {
                DESshifr.Key = Key;
                DESshifr.IV = IV;
                ICryptoTransform xfrm = DESshifr.CreateDecryptor();
                byte[] outBlock = xfrm.TransformFinalBlock(data, 0, data.Length);
                return Encoding.Default.GetString(outBlock);
            }
            catch
            {
                MessageBox.Show("Плохие данные!");
                return null;
            }
        }
    }



}
