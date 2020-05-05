using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Labs4Code.Static;
using Microsoft.Win32;

namespace Labs4Code.Model
{
    public class SteganographLabModel : PropertyChangedBase
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

        private int _seed = 0;
        /// <summary>
        /// Начальное значения Random (Сид)
        /// </summary>
        public int Seed
        {
            get => _seed;
            set
            {
                if (_seed != value)
                {
                    _seed = value;
                    NotifyOfPropertyChange(() => Seed);
                }
            }
        }

        private Bitmap PictureBitmap { get; set; }

        private Random Rand { get; set; }
        
        /// <summary>
        /// Количество бит внедряемой/извлекаемой информации.
        /// </summary>
        private int N { get; set; }

        public SteganographLabModel() { }

        /// <summary>
        /// Команда получения результата внедрения/извлечения
        /// </summary>
        public ICommand GetResultCommand
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    var paremater = (string)sender;
                    switch (paremater)
                    {
                        case "Insert":
                            {
                                if(TextIn != null)
                                {
                                    List<byte> encodeData = new List<byte>();
                                    encodeData.AddRange(Encoding.Unicode.GetBytes(TextIn));
                                    OpenPicture();
                                    if(Seed != 0)
                                    {
                                        N = InsertDataInPicture(encodeData, Seed, FilePath);
                                    }
                                    else MessageBox.Show("Задайте Seed!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                else MessageBox.Show("Введите сообщение", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                                break;
                            }
                        case "Extract":
                            {
                                OpenPicture();
                                TextOut = GetDataInPicture(N, Seed, FilePath);
                                break;
                            }
                    }
                });
            }
        }

        /// <summary>
        /// Метод внедрения информации в изображение
        /// </summary>
        /// <param name="insert_data">данные</param>
        /// <param name="seed">сид</param>
        /// <param name="filepath">путь до изображения</param>
        /// <returns></returns>
        private int InsertDataInPicture(List<byte> insert_data, int seed, string filepath)
        {
            PictureBitmap = new Bitmap(filepath);

            System.Drawing.Size pictureSize = PictureBitmap.Size;

            Rand = new Random(seed);

            List<Color> PixelInfo = new List<Color>();

            string BinInString = null;

            for (int i = 0; i < insert_data.Count; i++)
            {
                string strBin = Convert.ToString(insert_data[i], 2);
                strBin = strBin.PadLeft(8, '0');
                BinInString += strBin;
            }

            int n = BinInString.Length;

            for (int i = 0; i < BinInString.Length; i++)
            {
                int X_coordinate = Rand.Next(1, pictureSize.Width);
                int Y_coordinate = Rand.Next(1, pictureSize.Height);
                PixelInfo.Add(PictureBitmap.GetPixel(X_coordinate, Y_coordinate));
                int RGB_Component = Rand.Next(1, 3);

                switch (RGB_Component)
                {
                    case 1:
                        {
                            byte resultPixel = ResultValuePixel(PixelInfo[i].R, BinInString[i]);
                            PixelInfo[i] = Color.FromArgb(resultPixel, PixelInfo[i].G, PixelInfo[i].B);
                            break;
                        }
                    case 2:
                        {
                            byte resultPixel = ResultValuePixel(PixelInfo[i].G, BinInString[i]);
                            PixelInfo[i] = Color.FromArgb(PixelInfo[i].R, resultPixel, PixelInfo[i].B);
                            break;
                        }
                    case 3:
                        {
                            byte resultPixel = ResultValuePixel(PixelInfo[i].B, BinInString[i]);
                            PixelInfo[i] = Color.FromArgb(PixelInfo[i].R, PixelInfo[i].G, resultPixel);
                            break;
                        }

                }
                PictureBitmap.SetPixel(X_coordinate, Y_coordinate, PixelInfo[i]);
            }

            MessageBox.Show("Данные внедрены, запускаю процесс сохранения", "Работаем", MessageBoxButton.OK, MessageBoxImage.None);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.ShowDialog();
            PictureBitmap.Save(dialog.FileName, ImageFormat.Bmp);

            PictureBitmap.Dispose();

            return n;
        }
       
        /// <summary>
        /// Метод извлеения информации из изображения
        /// </summary>
        /// <param name="n">число битов информации</param>
        /// <param name="seed">сид</param>
        /// <param name="filepath">путь до изображения</param>
        /// <returns></returns>
        private string GetDataInPicture(int n, int seed, string filepath)
        {
            PictureBitmap = new Bitmap(filepath);

            System.Drawing.Size pictureSize = PictureBitmap.Size;

            Rand = new Random(seed);

            List<Color> PixelInfo = new List<Color>();

            List<byte> DataInfo = new List<byte>();

            string BitString = null;

            for (int i = 0; i < n; i++)
            {
                int X_coordinate = Rand.Next(1, pictureSize.Width);
                int Y_coordinate = Rand.Next(1, pictureSize.Height);

                PixelInfo.Add(PictureBitmap.GetPixel(X_coordinate, Y_coordinate));

                int RGB_Component = Rand.Next(1, 3);
                string infoBit = null;
                switch (RGB_Component)
                {
                    case 1:
                        {
                            infoBit = (PixelInfo[i].R & 0x1).ToString();
                            break;
                        }
                    case 2:
                        {
                            infoBit = (PixelInfo[i].G & 0x1).ToString();
                            break;
                        }
                    case 3:
                        {
                            infoBit = (PixelInfo[i].B & 0x1).ToString();
                            break;
                        }

                }
                BitString += infoBit;
            }

            byte[] ReciveByteData = new byte[BitString.Length / 8];

            for (int i = 0; i < ReciveByteData.Length; i++)
            {
                ReciveByteData[i] = Convert.ToByte(BitString.Substring(i * 8, 8), 2);
            }
            
            DataInfo.AddRange(ReciveByteData);

            PictureBitmap.Dispose();

            MessageBox.Show("Данные извлечены, вывожу информацию", "Работаем", MessageBoxButton.OK, MessageBoxImage.None);

            return Encoding.Unicode.GetString(DataInfo.ToArray());

        }

        /// <summary>
        /// Вставка младшего бита
        /// </summary>
        /// <param name="pixel_value">изменяемый пиксель</param>
        /// <param name="insert_value">вставляемое значение</param>
        /// <returns></returns>
        private byte ResultValuePixel(byte pixel_value, char insert_value)
        {
            if ((pixel_value % 2) == 0)
            {
                if (insert_value == '1') pixel_value += 1;
            }
            else
            {
                if (insert_value == '0') pixel_value -= 1;
            }
            return pixel_value;
        }

        /// <summary>
        /// Загрузка изображения
        /// </summary>
        private void OpenPicture()
        {
            var file = new OpenFileDialog();
            file.Filter = "text files (*.bmp)|*.bmp";
            if (file.ShowDialog() == true)
            {
                FilePath = file.FileName;
            }
            MessageBox.Show("Изображение загружено", "Состояние файла", MessageBoxButton.OK, MessageBoxImage.None);
        }

    }
}
