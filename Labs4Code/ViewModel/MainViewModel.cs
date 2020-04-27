using Labs4Code.Model;
using Labs4Code.Static;
using Labs4Code.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Labs4Code.ViewModel
{
    class MainViewModel
    {
        public CesarCodeModel CesarKrypt { get; set; }

        public VernanCodeModel VernamCrypt { get; set; }

        public BlockEncrypteModel DesCrypt { get; set; }

        public HashLabModel MD5HashLab { get; set; }


        public RSACodeModel RSALab { get; set; }


        public ShenksLabModel ShenksLab { get; set; }

        public MainViewModel()
        {
            ShenksLab = new ShenksLabModel();
        }

        public ICommand OpenLab1Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    CesarKrypt = new CesarCodeModel();
                    var openwindow = new CesarWindow()
                    {
                        DataContext = this,
                    };
                    openwindow.ShowDialog();
                });
            }
        }
        public ICommand OpenLab2Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    VernamCrypt = new VernanCodeModel();
                    DesCrypt = new BlockEncrypteModel();
                    var openwindow = new Lab2Window()
                    {
                        DataContext = this,
                    };
                    openwindow.ShowDialog();
                });
            }
        }

        public ICommand OpenLab3Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    MD5HashLab = new HashLabModel();

                    var openwindow = new Lab3Window()
                    {
                        DataContext = this,
                    };
                    openwindow.ShowDialog();
                });
            }
        }

        public ICommand OpenLab4Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    RSALab = new RSACodeModel();

                    var openwindow = new Lab4Window()
                    {
                        DataContext = this,
                    };
                    openwindow.ShowDialog();
                });
            }
        }
    }
}
