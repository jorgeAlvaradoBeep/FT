using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EFEarnings;
using Facturacion_Tostatronic.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;
using Facturacion_Tostatronic.ViewModels.Commands.SalesCommands;

namespace Facturacion_Tostatronic.ViewModels.Sales
{
    public class DialogComisionesVM:BaseNotifyPropertyChanged
    {
        #region Properties
        private bool gettingData;

		public bool GettingData
		{
			get { return gettingData; }
			set { SetValue(ref gettingData, value); }
		}
        public EFComisiones Comision { get; set; }
        private ObservableCollection<Plataforma> plataformasDisponibles;
        public ObservableCollection<Plataforma> PlataformasDisponibles
        {
            get { return plataformasDisponibles; }
            set { SetValue(ref plataformasDisponibles, value); }
        }
        private ObservableCollection<EFMetodoPago> metodosDePagoDisponibles;
        public ObservableCollection<EFMetodoPago> MetodosDePagoDisponibles
        {
            get { return metodosDePagoDisponibles; }
            set { SetValue(ref metodosDePagoDisponibles, value); }
        }
        #endregion
        #region Commands
        public VaseSaleComissionCommand VaseSaleComissionCommand { get; set; }
        #endregion
        public DialogComisionesVM()
        {
            Comision = new EFComisiones();
            Comision.VentaId = (int)Application.Current.Properties["Folio"];
            Application.Current.Properties["Folio"] = null;
            PlataformasDisponibles = new ObservableCollection<Plataforma>();
            MetodosDePagoDisponibles = new ObservableCollection<EFMetodoPago>();
            VaseSaleComissionCommand = new VaseSaleComissionCommand(this);
            CargarPlataformasDisponibles();
        }
        private async void CargarPlataformasDisponibles()
        {
            // Aquí deberías cargar las plataformas desde la base de datos
            // Este es un ejemplo, debes adaptarlo a cómo accedes a tu base de datos
            var res = await WebService.GetDataNode(URLData.Plataformas, "");
            if (!res.succes)
            {
                MessageBox.Show("Error al cargar las plataformas disponibles", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var plataformas = JsonConvert.DeserializeObject<List<Plataforma>>(res.data.ToString());
            foreach (var plataforma in plataformas)
            {
                PlataformasDisponibles.Add(plataforma);
            }

            res = await WebService.GetDataNode(URLData.MetodosDePago, "");
            if (!res.succes)
            {
                MessageBox.Show("Error al cargar los metodos de pago disponibles", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var metodos = JsonConvert.DeserializeObject<List<EFMetodoPago>>(res.data.ToString());
            foreach (var metodo in metodos)
            {
                MetodosDePagoDisponibles.Add(metodo);
            }
        }
    }
}
