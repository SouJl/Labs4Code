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
    public class ShenksLabModel:PropertyChangedBase
    {

        #region Входные данные (y,a,p)

        private string _y_inValue;
        public string Y_inValue
        {
            get => _y_inValue;
            set
            {
                if (_y_inValue != value)
                {
                    _y_inValue = value;
                    NotifyOfPropertyChange(() => Y_inValue);
                }
            }
        }

        private string _a_inValue;
        public string A_inValue
        {
            get => _a_inValue;
            set
            {
                if (_a_inValue != value)
                {
                    _a_inValue = value;
                    NotifyOfPropertyChange(() => A_inValue);
                }
            }
        }

        private string _p_inValue;
        public string P_inValue
        {
            get => _p_inValue;
            set
            {
                if (_p_inValue != value)
                {
                    _p_inValue = value;
                    NotifyOfPropertyChange(() => P_inValue);
                }
            }
        }

        #endregion

        #region Выходные данные (x)

        private int _x_outValue;
        public int X_outValue
        {
            get => _x_outValue;
            set
            {
                if (_x_outValue != value)
                {
                    _x_outValue = value;
                    NotifyOfPropertyChange(() => X_outValue);
                }
            }
        }

        #endregion
        
        #region Промежуточные вычисления 

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

        private int _k_Parametr;
        public int K_Parameter
        {
            get => _k_Parametr;
            set
            {
                if (_k_Parametr != value)
                {
                    _k_Parametr = value;
                    NotifyOfPropertyChange(() => K_Parameter);
                }
            }
        }

        private ObservableCollection<string> _firstRange;

        public ObservableCollection<string> FirstRange
        {
            get => _firstRange;
            set
            {
                if (_firstRange != value)
                {
                    _firstRange = value;
                    NotifyOfPropertyChange(() => FirstRange);
                }
            }
        }

        private ObservableCollection<string> _secondRange;

        public ObservableCollection<string> SecondRange
        {
            get => _secondRange;
            set
            {
                if (_secondRange != value)
                {
                    _secondRange = value;
                    NotifyOfPropertyChange(() => SecondRange);
                }
            }
        }

        #endregion



        public Random Rand { get; set; }


        public ShenksLabModel()
        {
            FullOvershoot(12, 13, 31);
        }


        public ICommand GetResultCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    /*y = (a^x)modp*/

                    int y_value = Convert.ToInt32(Y_inValue);
                    int a_value = Convert.ToInt32(A_inValue);
                    int p_value = Convert.ToInt32(P_inValue);

                    FirstStepSolve(p_value);
                  
                    SecondStepSolve(y_value, a_value, p_value);
                    
                    ThirdStepValue();

                });
            }
        }


        private void FirstStepSolve(int p_value) 
        {
            K_Parameter = (int)Math.Sqrt(p_value);
            M_Parameter = K_Parameter + 1;
            while (M_Parameter * K_Parameter < p_value)
                M_Parameter++;
        }

        private void SecondStepSolve(int y_value, int a_value, int p_value)
        {
            FirstRange = new ObservableCollection<string>();

            SecondRange = new ObservableCollection<string>();

            for (int i =0; i <= M_Parameter - 1; i++)
            {
                BigInteger new_y_value = new BigInteger(y_value);
                BigInteger new_p_value = new BigInteger(p_value);
                int result = (int)(BigInteger.Pow(a_value, i) * new_y_value % new_p_value);
                FirstRange.Add(result.ToString());
            }
            for (int i = 1; i <= K_Parameter - 1; i++)
            {
                BigInteger new_p_value = new BigInteger(p_value);
                int result = (int)(BigInteger.Pow(a_value, i * M_Parameter) % new_p_value);
                SecondRange.Add(result.ToString());
            }
        }

        private void ThirdStepValue()
        {
            int i = 0;
            int j = 1;
            
            foreach(var k_value in SecondRange)
            {
                if (FirstRange.Contains(k_value))
                {
                    i = FirstRange.IndexOf(k_value);
                    break; 
                }
                j++;
            }

            X_outValue = j * M_Parameter - i;
        }


        private void FullOvershoot(int y_value, int a_value, int p_value)
        {
            int i;
            for(i = 0; i <= p_value - 1; i++)
            {
                BigInteger new_p_value = new BigInteger(p_value);
                int result = (int)(BigInteger.Pow(a_value, i) % new_p_value);
                if (result == y_value) break;
            }
            X_outValue = i;
        }

    }
}
