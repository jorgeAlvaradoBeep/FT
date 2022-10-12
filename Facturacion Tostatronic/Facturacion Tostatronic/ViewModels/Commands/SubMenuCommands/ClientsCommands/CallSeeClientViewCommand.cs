﻿using Facturacion_Tostatronic.Views.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SubMenuCommands.ClientsCommands
{
    public class CallSeeClientViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SeeClients sC = new SeeClients();
            sC.ShowDialog();
        }
    }
}
