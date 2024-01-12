using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class SelectedDateChangedQuoteCommand : ICommand
    {
        public SeeQuatitionsVM VM { get; set; }

        public event EventHandler CanExecuteChanged;
        public SelectedDateChangedQuoteCommand(SeeQuatitionsVM vm)
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
            Response r = await WebService.GetDataNode(URLData.getDatedQuotesNET, VM.SelectedDate.Date.ToString("yyyy-MM-dd"));
            if (!r.succes)
            {
                if (!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error al traer datos de ventas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettinData = false;
                VM.Quatition = new ObservableCollection<CotizacionEF>();
                return;
            }

            VM.Quatition = JsonConvert.DeserializeObject<ObservableCollection<CotizacionEF>>(r.data.ToString());
            VM.GettinData = false;
        }
    }
}
