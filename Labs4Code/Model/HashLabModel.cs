using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Security.Cryptography;
using System.Windows.Input;
using Labs4Code.Static;
using System.Windows;
using System.Collections.ObjectModel;

namespace Labs4Code.Model
{
    /// <summary>
    /// 3 лабораторная работа (хэш-функция)
    /// </summary>
    public class HashLabModel:PropertyChangedBase
    {
        /// <summary>
        /// Хэш - функция
        /// </summary>
        private MD5 MD5HashFunc { get; set; }

        private Random Rand { get; set; }

        /// <summary>
        /// Входной список с числовой последоватльностью полученной через Rand.
        /// </summary>
        private List<int> RandList { get; set; }

        private ObservableCollection<int> _resultRandList;
        /// <summary>
        /// Выходной список с полученной псевдо-случайной последовательностью.
        /// </summary>
        public ObservableCollection<int> ResultRandList 
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


        public HashLabModel()
        {

        }

        /// <summary>
        /// Команда генерации последовательности
        /// </summary>
        public ICommand GetPSGCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    ResultRandList = new ObservableCollection<int>();
                    GetRandom();
                    GetHashList();
                });
            }
        }

        /// <summary>
        /// Команда удаление элементов из списка.
        /// </summary>
        public ICommand СleanListCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    if(ResultRandList.Any())
                        ResultRandList.Clear();
                    else
                        MessageBox.Show("Список пуст!");
                });
            }
        }

        /// <summary>
        /// Формирование списка входных значений.
        /// </summary>
        private void GetRandom()
        {
            Rand = new Random();
            RandList = new List<int>();
            if (Range != 0)
            {
                for (int i = 0; i < Range; i++)
                {
                    int r_value = Rand.Next(0, 255);
                    if (!RandList.Contains(r_value))
                        RandList.Add(r_value);
                    else
                    {
                        while (RandList.Contains(r_value)) r_value = Rand.Next(0, 255);
                        RandList.Add(r_value);
                    }
                }
            }
            else
            {
                MessageBox.Show("Ввидите количество элементов!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        /// <summary>
        /// Получения списка хэш-значений.
        /// </summary>
        private void GetHashList()
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


        /// <summary>
        /// Получение хэша от значения.
        /// </summary>
        /// <param name="md5Hash">хэш-функция</param>
        /// <param name="input">значение</param>
        /// <returns></returns>
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
