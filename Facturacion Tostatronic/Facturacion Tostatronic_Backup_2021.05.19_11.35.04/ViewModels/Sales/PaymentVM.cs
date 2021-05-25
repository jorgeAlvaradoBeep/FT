using Facturacion_Tostatronic.Models.Sales;
using Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands.PaymentCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Sales
{
    public class PaymentVM
    {
        #region Commands
        public SetPaymentCommand SetPaymentCommand { get; set; }
        public CancelPaymentCommand CancelPaymentCommand { get; set; }
        #endregion
        #region Properties
        public PaymentM Payment { get; set; }
        #endregion

        public PaymentVM()
        {
            Payment = new PaymentM((float)Application.Current.Properties["Total"]);
            Application.Current.Properties["Total"] = null;
            Application.Current.Properties["PaymentResult"] = null;
            SetPaymentCommand = new SetPaymentCommand(this);
            CancelPaymentCommand = new CancelPaymentCommand(this);
        }
    }
}
