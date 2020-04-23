using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

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

        public int P_Parameter { get; set; }
        public int Q_Parameter { get; set; }

        public int N_Parameter { get; set; }
        public int M_Parameter { get; set; }

        public int D_Parameter { get; set; }
        public int E_Parameter { get; set; }

        public Random Rand { get; set; }


        public RSACodeModel()
        {
            CalculateParams();
            
            List<byte> encodeData = new List<byte>();
            
            encodeData.AddRange(Encoding.Unicode.GetBytes("Как дела?"));
            
            List<string> encodeResult = Encode(encodeData, E_Parameter, N_Parameter);

            string decodeResult = Decode(encodeResult, D_Parameter, N_Parameter);


        } 

        public List<string> Encode(List<byte> data, int e_value, int n_value)
        {
            List<string> result = new List<string>();
            
            BigInteger bi;

            for (int i = 0; i < data.Count; i++)
            {
                bi = new BigInteger(data[i]);
                bi = BigInteger.Pow(bi, e_value);

                BigInteger n_ = new BigInteger(n_value);

                bi = bi % n_;
                result.Add(bi.ToString());
            }

            return result;
        }

        private string Decode(List<string> data, int d, int n)
        {           
            List<byte> decodeData = new List<byte>();
            
            BigInteger bi;

            foreach (string item in data)
            {
                bi = new BigInteger(Convert.ToDouble(item));
                bi = BigInteger.Pow(bi, d);

                BigInteger bi_n = new BigInteger(n);

                bi = bi % bi_n;

                decodeData.Add((byte)bi);

            }

            string result = Encoding.Unicode.GetString(decodeData.ToArray());
            return result;
        }

        #region Вычисления

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

        private int Calculate_D_Parameter(int invalue)
        {
            int result = invalue - 1;
            for (long i = 2; i <= invalue; i++)
            {
                if ((invalue % i == 0) && (result % i == 0))
                {
                    result--;
                    i = 1;
                }
            }
            return result;
        }

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
