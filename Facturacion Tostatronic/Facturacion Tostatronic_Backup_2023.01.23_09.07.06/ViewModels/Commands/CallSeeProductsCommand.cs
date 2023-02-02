﻿using Facturacion_Tostatronic.Views.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands
{
    public class CallSeeProductsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SeeProductsV sP = new SeeProductsV();
            sP.ShowDialog();
        }
    }
}