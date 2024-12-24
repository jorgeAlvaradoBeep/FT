using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Facturacion_Tostatronic.Views.Pages.Orders
{
    /// <summary>
    /// Lógica de interacción para OrderCheckUC.xaml
    /// </summary>
    public partial class OrderCheckUC : UserControl
    {
        public OrderCheckUC()
        {
            InitializeComponent();
        }
        private void RadAutoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var autoCompleteBox = sender as Telerik.Windows.Controls.RadAutoCompleteBox;
            // Suponiendo que el SelectedItem es un objeto que contiene el código del producto
            var selectedProductCode = (autoCompleteBox.SelectedItem as UpdateProductM)?.Codigo;

            if (selectedProductCode != null)
            {
                // Busca el primer elemento en los ítems del RadGridView que coincida con el código del producto seleccionado
                var itemToScrollTo = radGridView.Items.Cast<ProductOrderComplete>().FirstOrDefault(product => product.CodigoProducto == selectedProductCode);

                if (itemToScrollTo != null)
                {
                    // Si el producto existe, desplázate hasta él en el RadGridView
                    radGridView.ScrollIntoView(itemToScrollTo);

                    // Opcional: Realiza acciones adicionales después de desplazarte hasta el elemento, como seleccionarlo
                    radGridView.SelectedItem = itemToScrollTo;
                }
            }
        }

        private void RadNumericUpDown_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                if (this.DataContext is OrderCheckVM)
                {
                    if(((OrderCheckVM)this.DataContext).ComlpleteOrder != null)
                    {
                        ((OrderCheckVM)this.DataContext).ComlpleteOrder.GetSubTotalMxn();
                    }
                }
            }
        }

        private void radGridView_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            if(e.Cell.Column.UniqueName == "Cantidad" || e.Cell.Column.UniqueName == "Precio")
            {
                if (this.DataContext != null)
                {
                    if (this.DataContext is OrderCheckVM)
                    {
                        if (((OrderCheckVM)this.DataContext).ComlpleteOrder != null)
                        {
                            ((OrderCheckVM)this.DataContext).ComlpleteOrder.GetsubTotal();
                            ((OrderCheckVM)this.DataContext).ComlpleteOrder.GetSubTotalMxn();
                        }
                    }
                }
            }
        }
    }
}
