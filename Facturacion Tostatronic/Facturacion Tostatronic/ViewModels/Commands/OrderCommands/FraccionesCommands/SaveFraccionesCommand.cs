using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.FraccionesCommands
{
    public class SaveFraccionesCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public FraccionesArancelariasVM VM { get; set; }
        public SaveFraccionesCommand(FraccionesArancelariasVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            //Primero Agregamos las fracciones que no existen
            var fraccionesNuevas = VM.FraccionesOrden.Where(x => x.Nuevo).ToList();
            var fraccionesPorAgregar= fraccionesNuevas
            .Where(x => !string.IsNullOrEmpty(x.Composicion))
            .ToList();
            var fraccionesModificadas = VM.FraccionesOrden.Where(x => x.Modificado).ToList();
            string errMsg = "";
            string successMsg = "";
            VM.GettingData = true;
            Response res;
            if (fraccionesPorAgregar != null)
            {
                if(fraccionesPorAgregar.Count > 0)
                {
                    res = await WebService.InsertData(fraccionesPorAgregar, URLData.AgregarFraccionesArancelarias);
                    
                    if (res.succes)
                    {
                        successMsg += "Fracciones agregadas correctamente";
                        await Task.Run(() =>
                        {
                            foreach (var fraccion in fraccionesPorAgregar)
                            {
                                VM.FraccionesOrden.Where(x => x == fraccion).FirstOrDefault().Nuevo = false;
                                VM.FraccionesOrden.Where(x => x == fraccion).FirstOrDefault().Modificado = false;
                            }
                        });
                    }
                    else
                        errMsg += $"Error al agregar fracciones: {res.message}";
                }
            }
            //Ahora Modificamos las fracciones que ya existen
            if(fraccionesModificadas != null)
            {
                if (fraccionesModificadas.Count > 0)
                {
                    res = await WebService.ModifyData(fraccionesModificadas, URLData.ModificarFraccionesArancelarias);
                    if (res.succes)
                    {
                        await Task.Run(() =>
                        {
                            foreach (var fraccion in fraccionesModificadas)
                            {
                                VM.FraccionesOrden.Where(x => x == fraccion).FirstOrDefault().Modificado = false;
                            }
                        });
                    }
                    else
                        errMsg += $"{Environment.NewLine}Error al modificar fracciones: {res.message}";
                }
            }
            
            if (string.IsNullOrEmpty(errMsg))
                MessageBox.Show(successMsg, "Exito", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            else
            {
                if (string.IsNullOrEmpty(successMsg))
                    MessageBox.Show(errMsg, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                else
                    MessageBox.Show($"Errores: {errMsg}{Environment.NewLine}Exitos: {successMsg}", "Actualizado Con Errores", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
                
            VM.GettingData = false;
        }
    }
}
