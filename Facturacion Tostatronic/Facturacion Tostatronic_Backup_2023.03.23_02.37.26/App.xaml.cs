using Facturacion_Tostatronic.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    /// 


    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow app = new MainWindow();
            MenuVM context = new MenuVM();
            app.DataContext = context;
            app.Show();
        }
    }
}
