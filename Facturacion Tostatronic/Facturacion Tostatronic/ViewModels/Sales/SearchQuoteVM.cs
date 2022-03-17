using Facturacion_Tostatronic.Models.Sales;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.QuoteCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Sales
{
    public class SearchQuoteVM : BaseNotifyPropertyChanged
    {
        public SearchQuoteCommand SearchQuoteCommand { get; set; }
        public SearchQuoteDateCommand SearchQuoteDateCommand { get; set; }
        public QuoteSelectedCommand QuoteSelectedCommand { get; set; }

        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set { SetValue(ref gettingData, value); }
        }

        private string criterialSearch;

        public string CriterialSearch
        {
            get { return criterialSearch; }
            set { SetValue(ref criterialSearch, value); }
        }

        private Quotes quotes;

        public Quotes CompleteQuote
        {
            get { return quotes; }
            set { SetValue( ref quotes, value); }
        }

        private List<Quotes> listOfSearchedQuotes;

        public List<Quotes> ListOfSearchedQuotes
        {
            get { return listOfSearchedQuotes; }
            set { SetValue( ref listOfSearchedQuotes, value); }
        }

        private DateTime selectedDate;

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { SetValue(ref selectedDate, value); }
        }





        public SearchQuoteVM()
        {
            SearchQuoteCommand = new SearchQuoteCommand(this);
            SearchQuoteDateCommand = new SearchQuoteDateCommand(this);
            QuoteSelectedCommand = new QuoteSelectedCommand(this);

            GettingData = false;
            CompleteQuote = new Quotes();
            CompleteQuote.SaledProducts = new System.Collections.ObjectModel.ObservableCollection<Models.Products.ProductSaleSaled>();
            ListOfSearchedQuotes = new List<Quotes>();
            CriterialSearch = "";
            SelectedDate = DateTime.Now;
        }

    }
}
