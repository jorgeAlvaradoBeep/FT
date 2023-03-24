using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.SalesCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Sales
{
    public class SeeSalesVM : BaseNotifyPropertyChanged, IPageViewModel, IDataErrorInfo
    {
        public string Name { get; set; } = "SeeSalesVM";
        private bool gettinData;

		public bool GettinData
        {
			get { return gettinData; }
			set { SetValue(ref gettinData, value); }
		}

		private string saleID;

		public string SaleID
		{
			get { return saleID; }
			set 
			{
				int invoiceNumber;

                if (int.TryParse(value, out invoiceNumber))
				{
                    throw new ValidationException("No se aceptan letras en el folio");
                }
				SetValue(ref saleID, value); 
			}
		}

		private string folio;

		public string Folio
		{
			get { return folio; }
			set {
                SetValue(ref folio, value);
			}
		}

		private DateTime selectedDate;

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set 
			{ 
				SetValue(ref selectedDate,value); 
            }
        }

		private ObservableCollection<Sale> sales;

		public ObservableCollection<Sale> Sales
		{
			get { return sales; }
			set { SetValue(ref sales, value); }
		}

        //Region de comandos
        public SelectedDateChangedSaleCommand SelectedDateChangedSaleCommand { get; set; }
        public SearchSaleSSVCommand SearchSaleSSVCommand { get; set; }
        public SeeSalePDFCommand SeeSalePDFCommand { get; set; }
        public string Error
        {
            get
            {
                return ValidateText();
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Text": return this.ValidateText();
                }

                return null;
            }
        }

        public string ValidateText()
        {
            if (string.IsNullOrEmpty(this.Folio))
            {
                return "El folio no puede ir vacio";
            }
            else if(!int.TryParse(this.Folio, out int value))
            {
                return "No pueden haber letras en el folio.";
            }
            //else if (this.Text.Length < 5)
            //{
            //    return "Text cannot be less than 5 symbols.";
            //}

            return null;
        }

        public SeeSalesVM() 
		{
			GettinData = false;
            SelectedDate = DateTime.Now;

			//Region de declaraci[on de comandos
			SelectedDateChangedSaleCommand = new SelectedDateChangedSaleCommand(this);
            SearchSaleSSVCommand = new SearchSaleSSVCommand(this);
            SeeSalePDFCommand = new SeeSalePDFCommand(this);

			Sales = new ObservableCollection<Sale>();

            SelectedDateChangedSaleCommand.Execute(null);
        }
    }
}
