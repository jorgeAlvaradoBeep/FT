using Facturacion_Tostatronic.ViewModels.Commands.InvoiceConfigCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturacion_Tostatronic.ViewModels
{
    public class InvoiceConfigVM
    {
        public SetCertificateCommand SetCertificateCommand { get; set; }
        public SetKeyCommand SetKeyCommand { get; set; }
        public InvoiceConfigVM()
        {
            SetCertificateCommand = new SetCertificateCommand(this);
            SetKeyCommand = new SetKeyCommand(this);
        }

        public bool SetCertificate()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Llave (*.key)|*.key";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string root = Directory.GetCurrentDirectory() + @"/Fiel";
                    var fileName = openFileDialog.FileName;
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }
                    File.Copy(fileName, root + "/Key.key", true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SetKey()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Certificado (*.cer)|*.cer";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string root = Directory.GetCurrentDirectory() + @"/Fiel";
                    var fileName = openFileDialog.FileName;
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }
                    System.IO.File.Copy(fileName, root + "/Certifiado.cer", true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
