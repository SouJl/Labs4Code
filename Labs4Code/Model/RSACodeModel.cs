using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Labs4Code.Static;

namespace Labs4Code.Model
{
    public class RSACodeModel: PropertyChangedBase
    {
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


        private ObservableCollection<string> _encodeResult;
        /// <summary>
        /// Выходная последовательность чисел.
        /// </summary>
        public ObservableCollection<string> EncodeResult 
        {
            get => _encodeResult;
            set
            {
                if (_encodeResult != value)
                {
                    _encodeResult = value;
                    NotifyOfPropertyChange(() => EncodeResult);
                }
            }
        }


        public Random Rand { get; set; }

        #region Параметры RSA

        private int _p_Parametr;
        public int P_Parameter 
        {
            get => _p_Parametr;
            set
            {
                if (_p_Parametr != value)
                {
                    _p_Parametr = value;
                    NotifyOfPropertyChange(() => P_Parameter);
                }
            }
        }

        private int _q_Parametr;

        public int Q_Parameter 
        {
            get => _q_Parametr;
            set
            {
                if (_q_Parametr != value)
                {
                    _q_Parametr = value;
                    NotifyOfPropertyChange(() => Q_Parameter);
                }
            }
        }

        private int _n_Parametr;
        public int N_Parameter
        {
            get => _n_Parametr;
            set
            {
                if (_n_Parametr != value)
                {
                    _n_Parametr = value;
                    NotifyOfPropertyChange(() => N_Parameter);
                }
            }
        }
        private int _m_Parametr;
        public int M_Parameter
        {
            get => _m_Parametr;
            set
            {
                if (_m_Parametr != value)
                {
                    _m_Parametr = value;
                    NotifyOfPropertyChange(() => M_Parameter);
                }
            }
        }
        private int _d_Parametr;
        public int D_Parameter
        {
            get => _d_Parametr;
            set
            {
                if (_d_Parametr != value)
                {
                    _d_Parametr = value;
                    NotifyOfPropertyChange(() => D_Parameter);
                }
            }
        }

        private int _e_Parametr;
        public int E_Parameter
        {
            get => _e_Parametr;
            set
            {
                if (_e_Parametr != value)
                {
                    _e_Parametr = value;
                    NotifyOfPropertyChange(() => E_Parameter);
                }
            }
        }

        #endregion


        public RSACodeModel()
        {
        }

        /// <summary>
        /// Команда вычисления параметров RSA
        /// </summary>
        public ICommand CalculateParamsValueCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    CalculateParams();
                });
            }
        }

        /// <summary>
        /// Команда кодирования информации
        /// </summary>
        public ICommand EncodeCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    List<byte> encodeData = new List<byte>();
                    encodeData.AddRange(Encoding.Unicode.GetBytes(TextIn));
                    EncodeResult = Encode(encodeData, E_Parameter, N_Parameter);
                });
            }
        }

        /// <summary>
        /// Команда декодирвоания информации
        /// </summary>
        public ICommand DecodeCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    TextOut = Decode(EncodeResult, D_Parameter, N_Parameter); 
                });
            }
        }

        /// <summary>
        /// Метод кодирования входных данных
        /// </summary>
        /// <param name="data">кодируемые данные</param>
        /// <param name="e_value">значение e</param>
        /// <param name="n_value">значение n</param>
        /// <returns>выходная последовательность</returns>
        public ObservableCollection<string> Encode(List<byte> data, int e_value, int n_value)
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            
            for (int i = 0; i < data.Count; i++)
            {
                int val = ModePow(Convert.ToInt32(data[i]), e_value, n_value);
                result.Add(val.ToString());
            }
            return result;
        }


        /// <summary>
        /// Декодирование данных
        /// </summary>
        /// <param name="data">декодируемые данные</param>
        /// <param name="d">значение d</param>
        /// <param name="n">значение n</param>
        /// <returns>полученная информация</returns>
        private string Decode(ObservableCollection<string> data, int d, int n)
        {           
            List<byte> decodeData = new List<byte>();
           
            foreach (string item in data)
            {
                int val = ModePow(Convert.ToInt32(item), d, n);
                decodeData.Add((byte)val);

            }
            return Encoding.Unicode.GetString(decodeData.ToArray());
        }

        /// <summary>
        /// Быстрое возведение в степень.
        /// </summary>
        /// <param name="a">основание</param>
        /// <param name="x">степень</param>
        /// <param name="p">делитель</param>
        /// <returns></returns>
        private int ModePow(int a, int x, int p)
        {
            int result = 1;
            while (x != 0)
            {
                if (x % 2 != 0)
                {
                    result = (result * a) % p;
                }
                a = (a * a) % p;
                x /= 2;
            }
            return result;
        }


        #region Вычисления

        /// <summary>
        /// Расчет основных параметров
        /// </summary>
        private void CalculateParams()
        {
            Rand = new Random();
            P_Parameter = Rand.Next(2, 149);
            while (!IsTheNumberSimple(P_Parameter)) P_Parameter = Rand.Next(2, 149);
  
            Q_Parameter = Rand.Next(2, 149);
            while (!IsTheNumberSimple(Q_Parameter)) Q_Parameter = Rand.Next(2, 149);

            N_Parameter = P_Parameter * Q_Parameter;
            M_Parameter = (P_Parameter - 1) * (Q_Parameter - 1);

            D_Parameter = Calculate_D_Parameter(M_Parameter);

            E_Parameter = Calculate_E_Parametr(D_Parameter, M_Parameter);

        }

        /// <summary>
        /// Проверка на взаимнопростые
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool IsTheNumberSimple(int parameter)
        {
            var result = true;

            if (parameter > 1)
            {
                for (var i = 2u; i < parameter; i++)
                {
                    if (parameter % i == 0)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }

            return result;

        }

        /// <summary>
        ///  Нахождение параметра d
        /// </summary>
        /// <param name="m_value"></param>
        /// <returns></returns>
        private int Calculate_D_Parameter(int m_value)
        {
            int result = m_value - 1;
            for (long i = 2; i <= m_value; i++)
            {
                if ((m_value % i == 0) && (result % i == 0))
                {
                    result--;
                    i = 1;
                }
            }
            return result;
        }

        /// <summary>
        /// Нахождение параметра e
        /// </summary>
        /// <param name="d_value"></param>
        /// <param name="m_value"></param>
        /// <returns></returns>
        private int Calculate_E_Parametr(int d_value, int m_value)
        {
            int result = 10;
            while (true)
            {
                if ((result * d_value) % m_value == 1)
                    break;
                else
                    result++;
            }
            return result;
        }

        #endregion

    }
}
