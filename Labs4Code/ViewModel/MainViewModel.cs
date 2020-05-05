using Labs4Code.Model;
using Labs4Code.Static;
using Labs4Code.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;

namespace Labs4Code.ViewModel
{
    public class MainViewModel : PropertyChangedBase
    {
        private bool _isVisible = true;
        /// <summary>
        /// Отображение главного окна.
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    NotifyOfPropertyChange(() => IsVisible);
                }
            }
        }

        #region Объявление моделей лабораторных работ

        public CesarCodeModel CesarKrypt { get; set; }

        public VernanCodeModel VernamCrypt { get; set; }

        public BlockEncrypteModel DesCrypt { get; set; }

        public HashLabModel MD5HashLab { get; set; }


        public RSACodeModel RSALab { get; set; }

        public ShenksLabModel ShenksLab { get; set; }

        public BitCoinNonceModel BitNonceLAb { get; set; }
        
        public SteganographLabModel StenoGraphLab { get; set; }

        #endregion

        #region Объявление отображений лабораторных работ

        private CesarWindow Lab1Window { get; set; }
        private Lab2Window Lab2Window { get; set; }
        private Lab3Window Lab3Window { get; set; }
        private Lab4Window Lab4Window { get; set; }
        private Lab5Window Lab5Window { get; set; }
        private Lab6Window Lab6Window { get; set; }
        private Lab7Window Lab7Window { get; set; }
        private Lab8Window Lab8Window { get; set; }

        #endregion
        
        public MainViewModel() { }

        #region Команды открытия окон лабораторных работ

        public ICommand OpenLab1Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Lab1Window = new CesarWindow();
                    Lab1Window.Closed += HiddenWindowEvent;
                    CesarKrypt = new CesarCodeModel();
                    Lab1Window.DataContext = this;
                    IsVisible = false;
                    Lab1Window.ShowDialog();
                });
            }
        }
        public ICommand OpenLab2Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Lab2Window = new Lab2Window();
                    Lab2Window.Closed += HiddenWindowEvent;
                    VernamCrypt = new VernanCodeModel();
                    DesCrypt = new BlockEncrypteModel();
                    Lab2Window.DataContext = this;
                    IsVisible = false;
                    Lab2Window.ShowDialog();
                });
            }
        }

        public ICommand OpenLab3Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Lab3Window = new Lab3Window();
                    Lab3Window.Closed += HiddenWindowEvent;
                    MD5HashLab = new HashLabModel();
                    Lab3Window.DataContext = this;
                    IsVisible = false;
                    Lab3Window.ShowDialog();
                });
            }
        }

        public ICommand OpenLab4Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Lab4Window = new Lab4Window();
                    Lab4Window.Closed += HiddenWindowEvent;
                    RSALab = new RSACodeModel();
                    Lab4Window.DataContext = this;
                    IsVisible = false;
                    Lab4Window.ShowDialog();
                });
            }
        }

        public ICommand OpenLab5Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Lab5Window = new Lab5Window();
                    Lab5Window.Closed += HiddenWindowEvent;
                    IsVisible = false;
                    Lab5Window.ShowDialog();
                });
            }
        }


        public ICommand OpenLab6Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Lab6Window = new Lab6Window();
                    Lab6Window.Closed += HiddenWindowEvent;
                    ShenksLab = new ShenksLabModel();
                    Lab6Window.DataContext = this;
                    IsVisible = false;
                    Lab6Window.ShowDialog();
                });
            }
        }

        public ICommand OpenLab7Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Lab7Window = new Lab7Window();
                    Lab7Window.Closed += HiddenWindowEvent;
                    BitNonceLAb = new BitCoinNonceModel();
                    Lab7Window.DataContext = this;
                    IsVisible = false;
                    Lab7Window.ShowDialog();
                });
            }
        }

        public ICommand OpenLab8Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    Lab8Window = new Lab8Window();
                    Lab8Window.Closed += HiddenWindowEvent;
                    StenoGraphLab = new SteganographLabModel();
                    Lab8Window.DataContext = this;
                    IsVisible = false;
                    Lab8Window.ShowDialog();
                });
            }
        }

        #endregion
        
        /// <summary>
        /// Событие отображения главного окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HiddenWindowEvent(object sender, EventArgs e)
        {
            IsVisible = true;
        }

    }
}
