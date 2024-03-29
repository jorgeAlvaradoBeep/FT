﻿using Facturacion_Tostatronic.Views.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PricesCommands
{
    public class CreateMLPricesCommand : ICommand
    {
        public MenuVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public CreateMLPricesCommand(MenuVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //VM.GetMLPricesExcel();
            MLPriveV mlPrices = new MLPriveV();
            mlPrices.ShowDialog();
        }
    }
}
