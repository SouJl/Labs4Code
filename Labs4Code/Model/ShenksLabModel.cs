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

        private int _y_inValue = 0;
        public int Y_inValue
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

        private int _a_inValue = 0;
        public int A_inValue
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

        private int _p_inValue;
        public int P_inValue
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

        private int _x_outValue = 0;
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


        private int _x_OvershootoutValue = 0;
        public int X_OvershootoutValue
        {
            get => _x_OvershootoutValue;
            set
            {
                if (_x_OvershootoutValue != value)
                {
                    _x_OvershootoutValue = value;
                    NotifyOfPropertyChange(() => X_OvershootoutValue);
                }
            }
        }

        #endregion

        #region Промежуточные вычисления 

        private int _m_Parametr = 0;
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

        private int _k_Parametr = 0;
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

        private ObservableCollection<int> _firstRange;

        public ObservableCollection<int> FirstRange
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

        private ObservableCollection<int> _secondRange;

        public ObservableCollection<int> SecondRange
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

        private int _i_Parametr = 0;
        public int I_Parameter
        {
            get => _i_Parametr;
            set
            {
                if (_i_Parametr != value)
                {
                    _i_Parametr = value;
                    NotifyOfPropertyChange(() => I_Parameter);
                }
            }
        }

        private int _j_Parametr = 0;
        public int J_Parameter
        {
            get => _j_Parametr;
            set
            {
                if (_j_Parametr != value)
                {
                    _j_Parametr = value;
                    NotifyOfPropertyChange(() => J_Parameter);
                }
            }
        }


        private ObservableCollection<int> _i_Range;

        public ObservableCollection<int> I_Range
        {
            get => _i_Range;
            set
            {
                if (_i_Range != value)
                {
                    _i_Range = value;
                    NotifyOfPropertyChange(() => I_Range);
                }
            }
        }


        #endregion



        public Random Rand { get; set; }


        public ShenksLabModel()
        {
            //FullOvershoot(12, 13, 31);
        }


        public ICommand GetResultCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    var parameter = (string)sender;

                    switch (parameter)
                    {
                        case "Main":
                            {
                                /*y = (a^x)modp*/

                                FirstStepSolve(P_inValue);

                                SecondStepSolve(Y_inValue, A_inValue, P_inValue);

                                ThirdStepValue();
                                break;
                            }
                        case "Overshoot":
                            {
                                /*Полный перебор*/
                                I_Range = new ObservableCollection<int>();
                                FullOvershoot(Y_inValue, A_inValue, P_inValue);
                                break;
                            }
                    }
 
                });
            }
        }


        public ICommand CleanCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {

                    var parameter = (string)sender;

                    switch (parameter)
                    {
                        case "Main":
                            {
                                //Y_inValue = A_inValue = P_inValue = 0;
                                K_Parameter = M_Parameter = 0;
                                FirstRange.Clear();
                                SecondRange.Clear();
                                I_Parameter = J_Parameter = 0;
                                X_outValue = 0;
                                break;
                            }
                        case "Overshoot":
                            {
                                //Y_inValue = A_inValue = P_inValue = 0;
                                I_Range.Clear();
                                X_OvershootoutValue = 0;
                                break;
                            }
                    }


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
            FirstRange = new ObservableCollection<int>();

            SecondRange = new ObservableCollection<int>();

            for (int i =0; i <= M_Parameter - 1; i++)
            {
                BigInteger new_y_value = new BigInteger(y_value);
                BigInteger new_p_value = new BigInteger(p_value);
                int result = (int)(BigInteger.Pow(a_value, i) * new_y_value % new_p_value);
                FirstRange.Add(result);
            }
            for (int i = 1; i <= K_Parameter - 1; i++)
            {
                BigInteger new_p_value = new BigInteger(p_value);
                int result = (int)(BigInteger.Pow(a_value, i * M_Parameter) % new_p_value);
                SecondRange.Add(result);
            }
        }

        private void ThirdStepValue()
        {
            int i = 1;
            int j = 0;
            
            foreach(var k_value in SecondRange)
            {
                if (FirstRange.Contains(k_value))
                {
                    j = FirstRange.IndexOf(k_value);
                    break; 
                }
                i++;
            }
            
            I_Parameter = i;
            J_Parameter = j;
            
            X_outValue = I_Parameter * M_Parameter - J_Parameter;
        }


        private void FullOvershoot(int y_value, int a_value, int p_value)
        {
            int i;
            for(i = 0; i <= p_value - 1; i++)
            {
                BigInteger new_p_value = new BigInteger(p_value);
                int result = (int)(BigInteger.Pow(a_value, i) % new_p_value);
                I_Range.Add(i);
                if (result == y_value) break;
            }
            X_OvershootoutValue = i;
        }

    }
}
