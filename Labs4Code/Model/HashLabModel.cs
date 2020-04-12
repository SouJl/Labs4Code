using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Security.Cryptography;
using System.Windows.Input;
using Labs4Code.Static;

namespace Labs4Code.Model
{
    /// <summary>
    /// 3 лабораторная работа (хэш-функция)
    /// </summary>
    public class HashLabModel:PropertyChangedBase
    {
        private MD5 MD5HashFunc { get; set; }

        private List<int> RandList { get; set; }

        private List<int> _resultRandList;
        public List<int> ResultRandList 
        {
            get => _resultRandList;
            set
            {
                if(_resultRandList != value)
                {
                    _resultRandList = value;
                    NotifyOfPropertyChange(() => ResultRandList);
                }
            }
        }


        private int _range;
        /// <summary>
        /// Количество чисел.
        /// </summary>
        public int Range
        {
            get => _range;
            set
            {
                if (_range != value)
                {
                    _range = value;
                    NotifyOfPropertyChange(() => Range);
                }
            }
        }


        private const byte ValueCount = 10;

        private Random Rand { get; set; }

        public HashLabModel()
        {
            GetPSGCommand.Execute(0);
        }

        public ICommand GetPSGCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    ResultRandList = new List<int>();
                    GetRandom();
                    GetHash();
                });
            }
        }


        private void GetRandom()
        {
            Rand = new Random();
            RandList = new List<int>();
            int r_value = 0;
            for (int i = 0; i < ValueCount; i++)
            {
                r_value = Rand.Next(0, 255);
                if(!RandList.Contains(r_value))
                    RandList.Add(r_value);
                else
                {
                    while (RandList.Contains(r_value)) r_value = Rand.Next(0, 255);
                    RandList.Add(r_value);
                }
            }
        }



        private void GetHash()
        {
            using (MD5HashFunc = MD5.Create())
            {
                Rand = new Random();
                foreach (int value in RandList)
                {
                    string hash = GetMd5Hash(MD5HashFunc, value.ToString());
                    string hash_short = hash.Substring(Rand.Next(0, (hash.Length - 1)), 2);
                    int result = Convert.ToInt32(hash_short, 16);
                    ResultRandList.Add(result);
                }
            }
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

    }
}
