using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class VaseSaleComissionCommand:ICommand
    {
        public DialogComisionesVM VM { get; set; }
        public VaseSaleComissionCommand(DialogComisionesVM vm)
        {
            VM = vm;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.GettingData = true;
            // Aquí deberías guardar la comisión en la base de datos
            // Este es un ejemplo, debes adaptarlo a cómo accedes a tu base de datos
            var res = await WebService.InsertData(VM.Comision, URLData.Comisiones);
            if (!res.succes)
            {
                MessageBox.Show("Error al guardar la comisión", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                VM.GettingData = false;
                return;
            }
            MessageBox.Show("Comisión guardada correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            VM.GettingData = false;
        }
    }
}
