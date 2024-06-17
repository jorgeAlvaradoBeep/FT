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

namespace Facturacion_Tostatronic.Views.PDV.Sales
{
    /// <summary>
    /// Lógica de interacción para DialogComisiones.xaml
    /// </summary>
    public partial class DialogComisiones : Window
    {
        public DialogComisiones()
        {
            InitializeComponent();
        }
        private void DataSelect(object sender, RoutedEventArgs e)

        {

            TextBox tb = (sender as TextBox);

            if (tb != null)

            {

                tb.SelectAll();

            }

        }
    }
}
