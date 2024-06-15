using Facturacion_Tostatronic.Models.EF_Models.EFEarnings;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class SaveComissionCommand:ICommand
    {
        public EarningsVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SaveComissionCommand(EarningsVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.GettinData = true;
            if(VM.Sales.Count == 0)
            {
                MessageBox.Show("No hay ventas para guardar comisiones", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettinData = false;
                return;
            }
            await Task.Run(() => SaveComission());
            VM.GettinData = false;
        }

        async void SaveComission()
        {
            var comisiones = VM.Sales.Select(sale => sale.Comisiones).ToList();
            var comisionesJson = JsonConvert.SerializeObject(comisiones);
            Response r = await WebService.InsertData(comisiones,URLData.SaveComission);
            if (!r.succes)
            {
                if (!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error al guardar comisiones", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                MessageBox.Show("Comisiones guardadas correctamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
