using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Sales;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.Services.Ticket;
using Facturacion_Tostatronic.ViewModels.Sales;
using Facturacion_Tostatronic.Views.PDV.Sales;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class SaveSaleCommand : ICommand
    {
        public SaleVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SaveSaleCommand(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        DialogPaymentV paymentV;
        public void Execute(object parameter)
        {
            if(VM.CompleteSale.SaledProducts.Count==0)
            {
                MessageBox.Show("Carrito vacio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Application.Current.Properties["Total"] = VM.CompleteSale.Total;
            paymentV = new DialogPaymentV();
            paymentV.Closing += PaymentV_Closing;
            paymentV.ShowDialog();
        }

        private async void PaymentV_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PaymentM payment = (PaymentM)Application.Current.Properties["PaymentResult"];
            Application.Current.Properties["PaymentResult"] = null;
            if (payment!= null)
            {
                if(payment.Cancel)
                {
                    e.Cancel=true;
                    paymentV.Closing -= PaymentV_Closing;
                    return;
                }
                VM.CompleteSale.Payment = payment;
                VM.GettingData = true;
                VM.CompleteSale.SearchedProducts = new List<Models.Products.ProductSaleSearch>();
                Response res = await WebService.InsertData(VM.CompleteSale, URLData.sales_save);
                if (!res.succes)
                {
                    MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.GettingData = false;
                    paymentV.Closing -= PaymentV_Closing;
                    return;
                }
                else
                {
                    VM.CompleteSale.IDSale = int.Parse(res.message);
                    try
                    {
                        await Task.Run(() => EndSale());
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"Error al imprimir: {ex.Message + Environment.NewLine}La venta se guardo exitosamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                                           
                    VM.GettingData = false;
                    VM.InitializeCompleteSale();
                }
            }
            //else
            //{
            //    e.Cancel = true;
            //    return;
            //}
            paymentV.Closing -= PaymentV_Closing;
        }
        void printPDF(string pathF, string name)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = pathF;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.Arguments = ConfigurationManager.AppSettings.Get("PrinterName");

            Process p = new Process();
            p.StartInfo = info;
            p.Start();

            p.WaitForInputIdle();
            System.Threading.Thread.Sleep(3000);
            if (false == p.CloseMainWindow())
                p.Kill();

        }
        void EndSale()
        {
            if (PrintTicket.ImprimeTicket(VM.CompleteSale))
            {
                //string ticketFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //ticketFile = Path.Combine(ticketFile, "Tostatronic");
                //ticketFile = Path.Combine(ticketFile, VM.CompleteSale.IDSale.ToString() + ".pdf");
                //printPDF(ticketFile, VM.CompleteSale.IDSale.ToString() + ".pdf");
                //try
                //{
                //    File.Delete(ticketFile);
                //}
                //catch (Exception a)
                //{
                //    MessageBox.Show("Error al borrar el ticker, se tendra que eliminar manualmente"
                //        + Environment.NewLine
                //        + $"Error: {a.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
            }
        }
    }
}
