using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Facturacion_Tostatronic.Views
{
    public class ScrollToNewItemBehavior : Behavior<RadGridView> 
    {
        private RadGridView GridView
        {
            get
            {
                return this.AssociatedObject as RadGridView;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.GridView.Items.CollectionChanged += OnCollectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.GridView.Items.CollectionChanged -= OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                this.GridView.ScrollIntoViewAsync(args.NewItems[0], this.GridView.Columns[0], null);
            }
        }
    }
}
