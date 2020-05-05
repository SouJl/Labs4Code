using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Labs4Code.Static;

namespace Labs4Code.Model
{
    public class BitCoinNonceModel : PropertyChangedBase
    {
        #region Входные значения 

        private int _computeValue = 0;
        /// <summary>
        /// входное значения Value.
        /// </summary>
        public int ComputeValue
        {
            get => _computeValue;
            set
            {
                if (_computeValue != value)
                {
                    _computeValue = value;
                    NotifyOfPropertyChange(() => ComputeValue);
                }
            }
        }

        private int _countOfZero = 0;
        /// <summary>
        /// Количество 0.
        /// </summary>
        public int CountOfZero
        {
            get => _countOfZero;
            set
            {
                if (_countOfZero != value)
                {
                    _countOfZero = value;
                    NotifyOfPropertyChange(() => CountOfZero);
                }
            }
        }

        #endregion

        #region Выходные значения 

        private int _nonce = 0;
        /// <summary>
        /// Подобранные NONCE.
        /// </summary>
        public int Nonce
        {
            get => _nonce;
            set
            {
                if (_nonce != value)
                {
                    _nonce = value;
                    NotifyOfPropertyChange(() => Nonce);
                }
            }
        }

        private string _textOut;
        /// <summary>
        /// Последний хэш с заданым числом 0.
        /// </summary>
        public string TextOut
        {
            get => _textOut;
            set
            {
                if (_textOut != value)
                {
                    _textOut = value;
                    NotifyOfPropertyChange(() => TextOut);
                }
            }
        }

        private ObservableCollection<string> _hashResultCollection;
        /// <summary>
        /// Колекция вычисленных хэшей.
        /// </summary>
        public ObservableCollection<string> HashResultCollection
        {
            get => _hashResultCollection;
            set
            {
                if (_hashResultCollection != value)
                {
                    _hashResultCollection = value;
                    NotifyOfPropertyChange(() => HashResultCollection);
                }
            }
        }

        #endregion

        /// <summary>
        /// Алгоритм хэширования.
        /// </summary>
        private SHA256 Hash { get; set; }

        public BitCoinNonceModel() 
        {
            Hash = SHA256.Create();
        }

        /// <summary>
        /// Команда получения результата.
        /// </summary>
        public ICommand GetResultCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {

                    HashResultCollection = new ObservableCollection<string>();
                    TextOut = null;
                    GetZeroHahResult();
                });
            }
        }

        /// <summary>
        /// Команда очистки вычислений
        /// </summary>
        public ICommand CleanCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    HashResultCollection.Clear();
                    TextOut = null;
                });
            }
        }

        /// <summary>
        /// Побор NONCE с заданным числом 0.
        /// </summary>
        private void GetZeroHahResult()
        {
            bool endofHash = false;
            int nonce = 0;
            string BinResultStr = null;
            while (!endofHash)
            {
                int result = ComputeValue | nonce;
                byte[] array = BitConverter.GetBytes(result);
                byte[] hashResult = Hash.ComputeHash(Hash.ComputeHash(array));
                for (int i = 0; i < hashResult.Length; i++)
                {
                    string strBin = Convert.ToString(hashResult[i], 2);
                    strBin = strBin.PadLeft(8, '0'); 
                    BinResultStr += strBin;
                }
                HashResultCollection.Add(BinResultStr);
                int checkcount = 0;
                int zeroPos;
                for (zeroPos = 0; zeroPos < CountOfZero; zeroPos++)
                {
                    if (BinResultStr[zeroPos] == '0') checkcount++;
                }
                if (checkcount == CountOfZero && BinResultStr[zeroPos + 1] == '1')
                {
                    endofHash = true;
                    Nonce = nonce;
                    TextOut = BinResultStr;
                }
                else
                {
                    HashResultCollection.Add(" ");
                    BinResultStr = null;
                    nonce++;
                }

            }
        }

    }
}
