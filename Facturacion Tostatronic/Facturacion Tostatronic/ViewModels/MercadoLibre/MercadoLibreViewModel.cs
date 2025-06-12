using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EFEarnings;
using Facturacion_Tostatronic.Models.MercadoLibre;
using Facturacion_Tostatronic.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.MercadoLibre
{
    public class MercadoLibreViewModel : INotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "MercadoLibreViewModel";
        public ObservableCollection<MercadoLibreOrder> Ventas { get; set; }

        private ObservableCollection<MercadoLibreOrder> _ventasFiltradas;
        public ObservableCollection<MercadoLibreOrder> VentasFiltradas
        {
            get => _ventasFiltradas;
            set { _ventasFiltradas = value; OnPropertyChanged(); }
        }

        private string _filtroIdVenta;
        public string FiltroIdVenta
        {
            get => _filtroIdVenta;
            set
            {
                _filtroIdVenta = value;
                OnPropertyChanged();
                AplicarFiltro();
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }
        private int _paginaActual = 0;
        public int PaginaActual
        {
            get => _paginaActual;
            set { _paginaActual = value; OnPropertyChanged(); }
        }

        private int _totalPaginas = 1;
        public int TotalPaginas
        {
            get => _totalPaginas;
            set { _totalPaginas = value; OnPropertyChanged(); }
        }

        public ICommand CargarVentasCommand { get; }
        public ICommand SiguientePaginaCommand { get; }
        public ICommand AnteriorPaginaCommand { get; }

        public MercadoLibreViewModel()
        {
            Ventas = new ObservableCollection<MercadoLibreOrder>();
            CargarVentasCommand = new RelayCommand(async () => await CargarVentasAsync());
            SiguientePaginaCommand = new RelayCommand(async () =>
            {
                if (PaginaActual + 1 < TotalPaginas)
                {
                    PaginaActual++;
                    await CargarVentasAsync();
                }
            });

            AnteriorPaginaCommand = new RelayCommand(async () =>
            {
                if (PaginaActual > 0)
                {
                    PaginaActual--;
                    await CargarVentasAsync();
                }
            });

            _ = CargarVentasAsync();
        }

        private async Task CargarVentasAsync()
        {
            IsBusy = true;

            string accessToken = "APP_USR-8061163509017498-051212-10da2c22aa0771f9d530e72746793153-1522344523";
            string userId = "1522344523"; // Reemplaza con el user_id real
            string fromDate = "2025-01-19T00:00:00.000Z";

            var client = new RestClient("https://api.mercadolibre.com");
            var request = new RestRequest($"https://api.mercadolibre.com/orders/search?seller={userId}&access_token={accessToken}&order.date_created.from={fromDate}&shipping.logistic_type=fulfillment&offset={PaginaActual}", Method.GET);

            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    Error = (sender, args) =>
                    {
                        // Ignora errores de propiedades individuales
                        args.ErrorContext.Handled = true;
                    }
                };

                var data = JsonConvert.DeserializeObject<MercadoLibreOrdersRoot>(response.Content, settings);
                var root = data?.results ?? new List<MercadoLibreOrder>();
                if(TotalPaginas==0 || TotalPaginas == 1)
                    TotalPaginas = data?.paging.Total ?? 0;
                //var data = JsonConvert.DeserializeObject<MercadoLibreOrdersRoot>(response.Content);
                //var root = data?.results ?? new List<MercadoLibreOrder>();
                //Extraemos las ventas que ya estan dadas de alta
                Response res = await WebService.GetDataNode(URLData.GetComisionesPlataforma, "3");
                if(!res.succes)
                {
                    MessageBox.Show("Error al cargar las comisiones locales", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var comisionesLocales = JsonConvert.DeserializeObject<List<EFComisiones>>(res.data.ToString());
                Ventas.Clear();
                foreach (var orden in root)
                {
                    // Verifica si la orden ya está en la lista
                    if (!comisionesLocales.Any(c => c.FolioPlataforma.Trim() == orden.OrderId.ToString().Trim()))
                    {
                        Ventas.Add(orden);
                    }
                    else
                        Console.WriteLine($"La orden {orden.OrderId} ya está en la lista.");

                }
            }

            AplicarFiltro();
            IsBusy = false;
        }

        private void AplicarFiltro()
        {
            if (string.IsNullOrWhiteSpace(FiltroIdVenta))
            {
                VentasFiltradas = new ObservableCollection<MercadoLibreOrder>(Ventas);
            }
            else if (long.TryParse(FiltroIdVenta, out long idFiltrado))
            {
                var filtradas = Ventas.Where(v => v.OrderId.ToString().Contains(idFiltrado.ToString()));
                VentasFiltradas = new ObservableCollection<MercadoLibreOrder>(filtradas);
            }
            else
            {
                VentasFiltradas = new ObservableCollection<MercadoLibreOrder>();
            }
        }

        private string LeerTokenSeguro()
        {
            var encrypted = System.IO.File.ReadAllBytes("ml_token.dat");
            var decrypted = System.Security.Cryptography.ProtectedData.Unprotect(encrypted, null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
            return System.Text.Encoding.UTF8.GetString(decrypted);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;
        public async void Execute(object parameter) => await _execute();
    }
}
