using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFEarnings
{
    public class Plataforma: BaseNotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { SetValue(ref id, value); }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { SetValue(ref nombre, value); }
        }

        private float comision;
        public float Comision
        {
            get { return comision; }
            set { SetValue(ref comision, value); }
        }
    }
}
