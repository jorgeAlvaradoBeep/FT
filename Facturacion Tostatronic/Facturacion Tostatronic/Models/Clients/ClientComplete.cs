using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Clients
{
    public class ClientComplete : BaseNotifyPropertyChanged
    {
        private int idCliente;

        public int IdCliente
        {
            get { return idCliente; }
            set { SetValue(ref idCliente, value); }
        }
        private int idTipoCliente;

        public int IdTipoCliente
        {
            get { return idTipoCliente; }
            set { SetValue(ref idTipoCliente, value); }
        }
        private string nombres;

        public string Nombres
        {
            get { return nombres; }
            set { SetValue(ref nombres, value); }
        }

        private string apellidoPaterno;

        public string ApellidoPaterno
        {
            get { return apellidoPaterno; }
            set { SetValue(ref apellidoPaterno, value); }
        }
        private string apellidoMaterno;

        public string ApellidoMaterno
        {
            get { return apellidoMaterno; }
            set { SetValue(ref apellidoMaterno, value); }
        }
        private string rfc;

        public string Rfc
        {
            get { return rfc; }
            set { SetValue(ref rfc, value); }
        }
        private string telefono;

        public string Telefono
        {
            get { return telefono; }
            set { SetValue(ref telefono, value); }
        }
        private string domicilio;

        public string Domicilio
        {
            get { return domicilio; }
            set { SetValue(ref domicilio, value); }
        }
        private string codigoPostal;

        public string CodigoPostal
        {
            get { return codigoPostal; }
            set { SetValue(ref codigoPostal, value); }
        }
        private string colonia;

        public string Colonia
        {
            get { return colonia; }
            set { SetValue(ref colonia, value); }
        }
        private string correoElectronico;

        public string CorreoElectronico
        {
            get { return correoElectronico; }
            set { SetValue(ref correoElectronico, value); }
        }
        private string celular;

        public string Celular
        {
            get { return celular; }
            set { SetValue(ref celular, value); }
        }
        private string descripcion;

        public string Descripcion
        {
            get { return descripcion; }
            set { SetValue(ref descripcion, value); }
        }
        private string regimenFiscal;

        public string RegimenFiscal
        {
            get { return regimenFiscal; }
            set { SetValue(ref regimenFiscal, value); }
        }

        public bool Eliminado { get; set; }

        private string completeName;

        public string CompleteName
        {
            get 
            {
                completeName = Nombres;
                if(!string.IsNullOrEmpty(ApellidoPaterno))
                    completeName =  completeName + " " + ApellidoPaterno;
                if (!string.IsNullOrEmpty(ApellidoMaterno))
                    completeName = completeName + " " + ApellidoMaterno;
                return completeName; 
            }
        }

    }
}
