using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Facturacion_Tostatronic
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<NavigationViewItemModel> navigationViewItems;
        public MainWindow()
        {
            string prePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = System.IO.Path.Combine(prePath, "Tostatronic");
            StyleManager.ApplicationTheme = new CrystalTheme();
            InitializeComponent();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void OnNavigationViewLoaded(object sender, RoutedEventArgs e)
        {
            this.navigationViewItems = new List<NavigationViewItemModel>();
            this.navigationViewItems.AddRange(this.NavigationView.Items.OfType<NavigationViewItemModel>());
            // Expand first expandable item
            var inboxItem = this.NavigationView.ItemContainerGenerator.ContainerFromItem(this.NavigationView.Items[0]) as RadNavigationViewItem;
            inboxItem.IsExpanded = true;
        }

        private void OnSelectTwitterSubItemBtnClick(object sender, RoutedEventArgs e)
        {
            var childItems = new List<NavigationViewItemModel>();
            var inboxItem = this.navigationViewItems.FirstOrDefault(item => item.Title == "Inbox") as NavigationViewItemModel;
            childItems.AddRange(inboxItem.SubItems.OfType<NavigationViewItemModel>());
            this.NavigationView.SelectedItem = childItems.FirstOrDefault(item => item.Title == "Twitter");
        }

        private void OnSelectImportantItemBtnClick(object sender, RoutedEventArgs e)
        {
            this.NavigationView.SelectedItem = this.navigationViewItems.FirstOrDefault(item => item.Title == "Important");
        }

        private void OnClearSelectionBtnClick(object sender, RoutedEventArgs e)
        {
            this.NavigationView.SelectedItem = null;
        }

        private void OnNavigationViewItemClick(object sender, RoutedEventArgs e)
        {
            var clickedItem = e.OriginalSource as RadNavigationViewItem;
        }

        private void OnNavigationViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var currentlySelectedItem = e.AddedItems[0] as NavigationViewItemModel;
            }
        }

        private void OnNavigationViewItemExpanded(object sender, RoutedEventArgs e)
        {
            var expandedItem = e.OriginalSource as RadNavigationViewItem;
        }

        private void OnNavigationViewItemCollapsed(object sender, RoutedEventArgs e)
        {
            var collapsedItem = e.OriginalSource as RadNavigationViewItem;
        }

        private Visibility GetIconVisibility(object icon)
        {
            if (icon != null)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
    }
}
