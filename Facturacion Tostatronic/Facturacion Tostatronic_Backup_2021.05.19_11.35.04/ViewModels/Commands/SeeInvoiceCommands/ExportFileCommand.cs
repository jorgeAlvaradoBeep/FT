using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SeeInvoiceCommands
{
    public class ExportFileCommand : ICommand
    {
        public SeeInvoiceVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public ExportFileCommand(SeeInvoiceVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();

            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                if (VM.CopyFiles(new System.IO.DirectoryInfo(folderName), true))
                    MessageBox.Show("Archivos exportados con exito", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
