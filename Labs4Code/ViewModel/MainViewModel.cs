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

        public MainViewModel()
        {
            CesarKrypt = new CesarCodeModel();
        }

        public ICommand OpenLab1Command
        {
            get
            {
                return new RelayCommand(sender =>
                {
                    var openwindow = new CesarWindow()
                    {
                        DataContext = this,
                    };
                    openwindow.ShowDialog();
                });
            }
        }
    }
}
