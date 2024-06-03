using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.SalesCommands;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturacion_Tostatronic.ViewModels.Sales
{
    public class EarningsVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "EarningsVM";
        #region Propiedades
        private List<EarningSale> sales;

		public List<EarningSale> Sales
		{
			get { return sales; }
			set { SetValue(ref sales, value); }
		}
        private DateTime selectedDate;

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                SetValue(ref selectedDate, value);
            }
        }
        private bool gettinData;
        public bool GettinData
        {
            get { return gettinData; }
            set { SetValue(ref gettinData, value); }
        }
        private float totalEarnings;

        public float TotalEarnings
        {
            get { return totalEarnings; }
            set { SetValue(ref totalEarnings, value); }
        }
        private float totalVentas;

        public float TotalVentas
        {
            get { return totalVentas; }
            set { SetValue(ref totalVentas, value); }
        }
        private int numberOfSales;

        public int NumberOfSales
        {
            get { return numberOfSales; }
            set { SetValue(ref numberOfSales, value); }
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
                    bool fileRight = ValidateFile();
                    if (!fileRight)
                        MessageBox.Show("Error, el documento a seleccionar " +
                            "debe de ser un archivo excel.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        IsFileLoad = true;
                }));
            }
        }
        public int FilaInicio { get; set; }
        public int FilaFin { get; set; }

        public readonly SynchronizationContext _syncContext;

        #endregion

        #region Comandos
        public DaySalesSelectedDateCommand DaySalesSelectedDateCommand { get; set; }
        public ChangeDataInfoCommand ChangeDataInfoCommand { get; set; }
        public ImportCommissionFromExcelCommand ImportCommissionFromExcelCommand { get; set; }
        #endregion
        public EarningsVM()
        {
            DispatcherHelper.Initialize();
            _syncContext = SynchronizationContext.Current;
            Sales = new List<EarningSale>();
            GettinData = false;
            SelectedDate = DateTime.Now;

            DaySalesSelectedDateCommand = new DaySalesSelectedDateCommand(this);
            ChangeDataInfoCommand = new ChangeDataInfoCommand(this);
            ImportCommissionFromExcelCommand = new ImportCommissionFromExcelCommand(this);
            DaySalesSelectedDateCommand.Execute(this);
        }

        #region LecturaExcel
        bool ValidateFile()
        {
            GettinData = true;
            if (string.IsNullOrWhiteSpace(ImagePath))
            {
                return false;
            }

            string extension = Path.GetExtension(ImagePath).ToLower();
            GettinData = false;
            return extension == ".xlsx" || extension == ".xls";

        }
        #endregion
    }
}
