﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Facturacion_Tostatronic.Views.Pages.Products
{
    /// <summary>
    /// Lógica de interacción para DisscountPricesUC.xaml
    /// </summary>
    public partial class DisscountPricesUC : UserControl
    {
        public DisscountPricesUC()
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