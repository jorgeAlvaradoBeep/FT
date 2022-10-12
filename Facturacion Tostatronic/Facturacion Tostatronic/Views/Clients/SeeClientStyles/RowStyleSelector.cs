using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Facturacion_Tostatronic.Views.Clients.SeeClientStyles
{
    public class RowStyleSelector: StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (((GridViewRow)container).GridViewDataControl.Items.IndexOf(item) == 0)
            {
                Style style = new Style(typeof(GridViewRow)) { BasedOn = (Style)Application.Current.Resources["GridViewRowStyle"] };
                Setter setter = new Setter(GridViewRow.DetailsVisibilityProperty, Visibility.Visible);
                style.Setters.Add(setter);
                return style;
            }

            return new Style(typeof(GridViewRow)) { BasedOn = (Style)Application.Current.Resources["GridViewRowStyle"] };
        }
    }
}
