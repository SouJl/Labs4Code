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
    public class BiCoinNonceModel : PropertyChangedBase
    {
        #region Входные значения 

        private int _computeValue = 0;
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

        private SHA256 Hash { get; set; }


        public BiCoinNonceModel()
        {
            Hash = SHA256.Create();
            HashResultCollection = new ObservableCollection<string>();
        }


        public ICommand GetResultCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    GetZeroHahResult();
                });
            }
        }

        private void GetZeroHahResult()
        {
            bool endofHash = false;
            int nonce = 0;
            string strAllbin = null;
            while (!endofHash)
            {
                int result = ComputeValue | nonce;
                byte[] array = BitConverter.GetBytes(result);
                byte[] hashResult = Hash.ComputeHash(Hash.ComputeHash(array));
                for (int i = 0; i < hashResult.Length; i++)
                {
                    string strBin = Convert.ToString(hashResult[i], 2);
                    strBin = strBin.PadLeft(8, '0');  // Zero Pad
                    strAllbin += strBin;
                }
                HashResultCollection.Add(strAllbin);
                int checkcount = 0;
                int zeroPos;
                for (zeroPos = 0; zeroPos < CountOfZero; zeroPos++)
                {
                    if (strAllbin[zeroPos] == '0') checkcount++;
                }
                if (checkcount == CountOfZero && strAllbin[zeroPos + 1] == '1')
                {
                    endofHash = true;
                    Nonce = nonce;
                    TextOut = strAllbin;
                }
                else
                {
                    HashResultCollection.Add(" ");
                    strAllbin = null;
                    nonce++;
                }

            }
        }

    }
}
