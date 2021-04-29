using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.PrinterConfigCommands;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels
{
    public class PrinterConfigVM : BaseNotifyPropertyChanged
    {
		private List<Printer> printers;

		public SetPrinterCommand SetPrinterCommand { get; set; }

		public List<Printer> Printers
		{
			get { return printers; }
			set { SetValue(ref printers,value); }
		}
		public PrinterConfigVM()
		{
			SetPrinterCommand = new SetPrinterCommand(this);
			Printers = GetPrintersName();
		}

		List<Printer> GetPrintersName()
		{
			List<Printer> names = new List<Printer>();
			foreach (string printerName in PrinterSettings.InstalledPrinters)
			{
				names.Add(new Printer() { PrinterName = printerName });
			}
			return names;
		}
	}
}
