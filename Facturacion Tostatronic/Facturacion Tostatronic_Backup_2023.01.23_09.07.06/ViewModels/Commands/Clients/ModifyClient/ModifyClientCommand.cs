using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Clients;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Facturacion_Tostatronic.ViewModels.Commands.Clients.ModifyClient
{
    public class ModifyClientCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SeeClientVM VM { get; set; }
        public ModifyClientCommand(SeeClientVM vM)
        {
            VM = vM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            ClientComplete cC = (ClientComplete)((RadTabItem)parameter).DataContext;
            if(cC != null)
            {
                if(string.IsNullOrEmpty(cC.Nombres))
                {
                    MessageBox.Show("El nombre del cliente no puede ir vacio","Error De dato",MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }
                //if (string.IsNullOrEmpty(cC.CodigoPostal))
                //{
                //    MessageBox.Show("El CP del cliente no puede ir vacio", "Error De dato", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}
                //if(cC.CodigoPostal.Length<5)
                //{
                //    MessageBox.Show("El CP del cliente no puede tener menos de 5 números", "Error De dato", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}
                if (!Regex.IsMatch(cC.CodigoPostal, @"^[0-9]{5}$"))
                {
                    MessageBox.Show("El CP del cliente no coincide con el formaro: 5 digitos numéricos ", "Error De dato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                VM.GettingData=true;
                Response res = await WebService.ModifyData(cC, URLData.editClient+cC.IdCliente.ToString());
                if (!res.succes)
                {
                    MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.GettingData = false;
                    return;
                }
                else
                {
                    VM.GettingData = false;
                    MessageBox.Show($"Cliente Modificado Con Exito", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
