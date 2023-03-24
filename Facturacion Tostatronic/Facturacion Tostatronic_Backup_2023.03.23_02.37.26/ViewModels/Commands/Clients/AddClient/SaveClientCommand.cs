using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Clients.AddClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.Clients.AddClient
{
    public class SaveClientCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public AddClientVM VM { get; set; }

        public SaveClientCommand(AddClientVM vM)
        {
            VM = vM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(string.IsNullOrEmpty(VM.Client.Nombres))
            {
                MessageBox.Show("El nombre o Razon social no pueden estar vacios","Error de llenado",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(VM.Client.Rfc))
            {
                MessageBox.Show("El RFC no pueden estar vacios", "Error de llenado", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (VM.Client.IdTipoCliente==0)
            {
                MessageBox.Show("Debe seleccionar un tipo de cliente", "Error de llenado", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(VM.Client.CodigoPostal))
            {
                MessageBox.Show("El codigo postal no puede dejarse vacio", "Error de llenado", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            VM.GettingData = true;
            if (VM.Client.ApellidoPaterno == null)
                VM.Client.ApellidoPaterno = ""; 
            if (VM.Client.ApellidoMaterno == null)
                VM.Client.ApellidoMaterno = ""; 
            if (VM.Client.Telefono == null)
                VM.Client.Telefono = ""; 
            if (VM.Client.Domicilio == null)
                VM.Client.Domicilio = ""; 
            if (VM.Client.Colonia == null)
                VM.Client.Colonia = "";
            if (VM.Client.CorreoElectronico == null)
                VM.Client.CorreoElectronico = "";
            if (VM.Client.Celular == null)
                VM.Client.Celular = "";
            if (VM.Client.Descripcion == null)
                VM.Client.Descripcion = "";
            if (VM.Client.RegimenFiscal == null)
                VM.Client.RegimenFiscal = "612";
            VM.Client.Rfc = VM.Client.Rfc.ToUpper();
            Response res = await WebService.GetDataNode(URLData.rfcExist, VM.Client.Rfc);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            res = await WebService.InsertData(VM.Client, URLData.addClient);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            else
            {
                VM.GettingData = false;
                MessageBox.Show($"Cliente Agregado Con Exito", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);
                VM.Client = new Models.Clients.ClientComplete();
                VM.RegimenFiscalSelected = null;
                VM.RegimenFiscalRequired=false;
                VM.SelectedClientType = null;
            }
        }
    }
}
