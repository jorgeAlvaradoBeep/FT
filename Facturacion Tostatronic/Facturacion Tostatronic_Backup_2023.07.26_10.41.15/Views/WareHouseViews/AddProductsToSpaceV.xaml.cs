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

namespace Facturacion_Tostatronic.Views.WareHouseViews
{
    /// <summary>
    /// Lógica de interacción para AddProductsToSpaceV.xaml
    /// </summary>
    public partial class AddProductsToSpaceV : Page
    {
        public AddProductsToSpaceV()
        {
            InitializeComponent();
        }

        public static implicit operator ContentControl(AddProductsToSpaceV v)
        {
            throw new NotImplementedException();
        }
    }
}
