using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturacion_Tostatronic.ViewModels.Orders
{
    public class ImportFromExcelVM : BaseNotifyPropertyChanged
    {
        #region Properties
        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set { SetValue(ref gettingData, value); }
        }
        private bool isFileLoad;

        public bool IsFileLoad
        {
            get { return isFileLoad; }
            set { SetValue(ref isFileLoad, value); }
        }
        public bool SelectFileAvailable
        {
            get { return !IsFileLoad; }
        }
        private bool isImportDone;

        public bool IsImportDone
        {
            get { return isImportDone; }
            set { SetValue(ref isImportDone, value); }
        }
        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set 
            {
                SetValue(ref imagePath, value);
                if (string.IsNullOrEmpty(imagePath))
                    return;
                Task.Run((() =>
                {
                    bool fileRight =ValidateFile();
                    if(!fileRight)
                        MessageBox.Show("Error, el documento a seleccionar " +
                            "debe de ser un archivo excel.","Error de formato",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        IsFileLoad = true;
                }));
            }
        }

        public ExcelParametersM ExcelFile { get; set; }

        #endregion
        #region Commands
        #endregion
        
        public ImportFromExcelVM()
        {
            ExcelFile = new ExcelParametersM();
            GettingData = false;
            IsFileLoad = false;
            IsImportDone = false;
        }

        bool ValidateFile()
        {
            GettingData = true;
            if (string.IsNullOrWhiteSpace(ImagePath))
            {
                return false;
            }

            string extension = Path.GetExtension(ImagePath).ToLower();
            GettingData = false;
            return extension == ".xlsx" || extension == ".xls";
            
        }
    }
}
