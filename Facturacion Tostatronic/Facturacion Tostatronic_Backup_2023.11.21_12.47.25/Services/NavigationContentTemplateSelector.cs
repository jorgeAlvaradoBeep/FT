using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Facturacion_Tostatronic.Services
{
    public class NavigationContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template { get; set; }
        public DataTemplate TemplateAlternative { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                NavigationViewItemModel model = (NavigationViewItemModel)item;
                if (!string.IsNullOrEmpty(model.Title))
                {
                    return this.TemplateAlternative;
                }
            }

            return this.Template;
        }
    }
}
