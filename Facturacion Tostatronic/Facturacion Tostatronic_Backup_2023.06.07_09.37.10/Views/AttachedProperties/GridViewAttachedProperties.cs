using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;

namespace Facturacion_Tostatronic.Views.AttachedProperties
{
    public class GridViewAttachedProperties
    {
        public static bool GetScrollToNewItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollToNewItemProperty);
        }

        public static void SetScrollToNewItem(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollToNewItemProperty, value);
        }

        public static readonly DependencyProperty ScrollToNewItemProperty =
            DependencyProperty.RegisterAttached("ScrollToNewItem", typeof(bool), typeof(GridViewAttachedProperties), new PropertyMetadata(false, OnScrollNewItemIntoViewChanged));

        private static void OnScrollNewItemIntoViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gridView = d as RadGridView;
            if (gridView != null)
            {
                var scrollToNewItem = GetScrollToNewItem(d);
                var behaviors = Interaction.GetBehaviors(d);
                var scrollToNewItemBehavior = behaviors.SingleOrDefault(x => x is ScrollToNewItemBehavior);

                if (scrollToNewItemBehavior != null && !scrollToNewItem)
                {
                    behaviors.Remove(scrollToNewItemBehavior);
                }
                else if (scrollToNewItemBehavior == null && scrollToNewItem)
                {
                    scrollToNewItemBehavior = new ScrollToNewItemBehavior();
                    behaviors.Add(scrollToNewItemBehavior);
                }
            }
        }
    }
}
