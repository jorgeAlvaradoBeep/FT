using Facturacion_Tostatronic.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SeeInvoiceCommands
{
    public class SendMailCommand : ICommand
    {
        public SeeInvoiceVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SendMailCommand(SeeInvoiceVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            string prePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(prePath, "Tostatronic");
            string pdfPath = @path + "\\" + VM.Folio + ".pdf";
            string xmlPath = @path + "\\" + VM.Folio + ".xml";
            string[] emails = VM.Email.Split(';');
            string result = await Task.Run(() => Email(emails, xmlPath, pdfPath));
            if (!string.IsNullOrEmpty(result))
                MessageBox.Show(result, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("Correo Enviado correctamente");
        }
        private static string Email(string[] emails, string pXMl, string pPDF)
        {
            if(emails.Length==0)
            {
                return "El correo no puede estar vacio";
            }
            try
            {
                Facturacion.Email(emails, pXMl, pPDF);
            }
            catch(Exception e)
            {
                return $"Error al enviar el correo: {e.Message}";
            }
            
            return "";
        }
        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }
    }
}
