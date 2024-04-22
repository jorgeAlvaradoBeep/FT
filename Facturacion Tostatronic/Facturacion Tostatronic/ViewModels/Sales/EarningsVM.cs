using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.SalesCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        #endregion

        #region Comandos
        public DaySalesSelectedDateCommand DaySalesSelectedDateCommand { get; set; }
        public ChangeDataInfoCommand ChangeDataInfoCommand { get; set; }
        #endregion
        public EarningsVM()
        {
            Sales = new List<EarningSale>();
            GettinData = false;
            SelectedDate = DateTime.Now;

            DaySalesSelectedDateCommand = new DaySalesSelectedDateCommand(this);
            ChangeDataInfoCommand = new ChangeDataInfoCommand(this);
            DaySalesSelectedDateCommand.Execute(this);
        }
    }
}
