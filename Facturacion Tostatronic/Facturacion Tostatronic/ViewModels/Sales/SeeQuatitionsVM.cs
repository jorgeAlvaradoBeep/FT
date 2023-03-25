using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.SalesCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Sales
{
    public class SeeQuatitionsVM : BaseNotifyPropertyChanged, IPageViewModel, IDataErrorInfo
    {
        public string Name { get; set; } = "SeeQuatitionsVM";
        private bool gettinData;

        public bool GettinData
        {
            get { return gettinData; }
            set { SetValue(ref gettinData, value); }
        }

        private string quatitionID;

        public string QuatitionID
        {
            get { return quatitionID; }
            set
            {
                int invoiceNumber;

                if (int.TryParse(value, out invoiceNumber))
                {
                    throw new ValidationException("No se aceptan letras en el folio");
                }
                SetValue(ref quatitionID, value);
            }
        }

        private string folio;

        public string Folio
        {
            get { return folio; }
            set
            {
                SetValue(ref folio, value);
            }
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

        private ObservableCollection<CotizacionEF> quatition;

        public ObservableCollection<CotizacionEF> Quatition
        {
            get { return quatition; }
            set { SetValue(ref quatition, value); }
        }

        //Region de comandos
        public SelectedDateChangedQuoteCommand SelectedDateChangedQuoteCommand { get; set; }
        public SearchQuoteSSVCommand SearchQuoteSSVCommand { get; set; }
        public SeeQuotePDFCommand SeeQuotePDFCommand { get; set; }
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
            else if (!int.TryParse(this.Folio, out int value))
            {
                return "No pueden haber letras en el folio.";
            }
            //else if (this.Text.Length < 5)
            //{
            //    return "Text cannot be less than 5 symbols.";
            //}

            return null;
        }

        public SeeQuatitionsVM()
        {
            GettinData = false;
            SelectedDate = DateTime.Now;

            //Region de declaraci[on de comandos
            SelectedDateChangedQuoteCommand = new SelectedDateChangedQuoteCommand(this);
            SearchQuoteSSVCommand = new SearchQuoteSSVCommand(this);
            SeeQuotePDFCommand = new SeeQuotePDFCommand(this);

            Quatition = new ObservableCollection<CotizacionEF>();

            SelectedDateChangedQuoteCommand.Execute(null);
        }
    }
}
