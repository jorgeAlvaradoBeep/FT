using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.SeeInvoiceCommands;
using Facturacion_Tostatronic.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawPrint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels
{
    public class SeeInvoiceVM : BaseNotifyPropertyChanged
    {
        #region Propiedades
        private string invoiceNumber;

        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { SetValue(ref invoiceNumber, value); }
        }

        private DateTime selectedDate;

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; }
        }
        private List<EndSale> sales;

        public List<EndSale> Sales
        {
            get { return sales; }
            set { SetValue(ref sales, value); }
        }
        private bool dataEntranceAvailable;

        public bool DataEntranceSavailable
        {
            get { return dataEntranceAvailable; }
            set { SetValue(ref dataEntranceAvailable, value); }
        }

        private string rfc;

        public string Rfc
        {
            get { return rfc; }
            set { SetValue(ref rfc, value); }
        }
        private string razonSocial;

        public string RazonSocial
        {
            get { return razonSocial; }
            set { SetValue(ref razonSocial,value); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { SetValue(ref email,value); }
        }

        private string folio;

        public string Folio
        {
            get { return folio; }
            set { folio = value; }
        }
        private bool isDataLoaded;

        public bool IsDataLoaded
        {
            get { return isDataLoaded; }
            set { SetValue(ref isDataLoaded, value); }
        }

        #endregion

        #region Commandos
        //Aqui agregamos todos los comandos
        public SelectedDateChangedSICommand SelectedDateChangedSICommand { get; set; }
        public SearchSaleSICommand SearchSaleSICommand { get; set; }
        public SaleSelectedSICommand SaleSelectedSICommand { get; set; }
        public SendMailCommand SendMailCommand { get; set; }
        public CloseFormCommand CloseFormCommand { get; set; }
        public ExportFileCommand ExportFileCommand { get; set; }
        public PrintInvoiceCommand PrintInvoiceCommand { get; set; }
        #endregion
        public SeeInvoiceVM()
        {
            Sales = new List<EndSale>();
            SelectedDate = DateTime.Now;
            DataEntranceSavailable = false;
            IsDataLoaded = true;

            #region CommandsInitalization
            //Aqui inicializamos los comandos
            SelectedDateChangedSICommand = new SelectedDateChangedSICommand(this);
            SearchSaleSICommand = new SearchSaleSICommand(this);
            SaleSelectedSICommand = new SaleSelectedSICommand(this);
            SendMailCommand = new SendMailCommand(this);
            CloseFormCommand = new CloseFormCommand(this);
            ExportFileCommand = new ExportFileCommand(this);
            PrintInvoiceCommand = new PrintInvoiceCommand(this);
            #endregion
        }

        public async void GetSalesData(string objectName, string keyString)
        {
            int invoiceNumber;
            if (int.TryParse(InvoiceNumber, out invoiceNumber))
            {
                WaitPlease wp = new WaitPlease();
                wp.Show();
                Response r = await WebService.GetData(objectName, keyString, URLData.factured_sales);
                if (r.statusCode == 404)
                {
                    if (!string.IsNullOrEmpty(r.message))
                        MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        MessageBox.Show("No existen registros de ese folio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    wp.Close();
                    Sales = new List<EndSale>();
                    return;
                }
                var t = r.data;
                Sales = JsonConvert.DeserializeObject<List<EndSale>>(t.ToString());
                wp.Close();
            }
            else
                MessageBox.Show("El campo de busqueda esta vacio.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public async void GetSalesDataByDate(string objectName, string keyString)
        {
            WaitPlease wp = new WaitPlease();
            wp.Show();
            Response r = await WebService.GetData(objectName, keyString, URLData.factured_sales);
            InvoiceNumber = string.Empty;
            if (r.statusCode == 404)
            {
                if (!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("No existen registros de esa fecha", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                wp.Close();
                Sales = new List<EndSale>();
                return;
            }
            var t = r.data;
            Sales = JsonConvert.DeserializeObject<List<EndSale>>(t.ToString());
            wp.Close();
        }

        public async Task<bool> GenerateInvoice(byte[] cXml, string folio)
        {
            
            return await Facturacion.GenerateInvoiceFiles(cXml, folio);
            
        }
        public bool CopyFiles(DirectoryInfo destination, bool overwrite)
        {
            string prePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(prePath, "Tostatronic");
            string pdfPath = @path + "\\" + Folio + ".pdf";
            string xmlPath = @path + "\\" + Folio + ".xml";
            FileInfo[] files = {new FileInfo(pdfPath),new FileInfo(xmlPath) };

            //this section is what's really important for your application.
            try
            {
                foreach (FileInfo file in files)
                {
                    file.CopyTo(destination.FullName + "\\" + file.Name, overwrite);
                }
                return true;
            }
            catch( Exception e)
            {
                MessageBox.Show($"Error: {e.Message}", "Error al exportar archivos", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }
        public bool PrintInvoice()
        {
            string prePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(prePath, "Tostatronic");
            string pdfPath = @path + "\\" + Folio + ".pdf";

            // Print the file
            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = false,
                    Verb = "print",
                    FileName = pdfPath //put the correct path here
                };
                p.Start();
                return true;
            }
            catch( Exception e)
            {
                MessageBox.Show($"Error: {e.Message}", "Error al imprimir factura", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }
    }
}
