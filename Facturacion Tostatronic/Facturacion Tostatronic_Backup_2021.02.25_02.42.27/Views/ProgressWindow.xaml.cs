using Facturacion_Tostatronic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Facturacion_Tostatronic.Views
{
    /// <summary>
    /// Lógica de interacción para ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public ProgressWindow()
        {
            InitializeComponent();
            pbStatus.Value = 0;
            //DataContext = new TaskProgressVM(ref progress);
        }
        public void changeProgress()
        {
            pbStatus.Value++;
        }
        public void changeProgress(int progress)
        {
            pbStatus.Value = progress;
        }
    }
}
