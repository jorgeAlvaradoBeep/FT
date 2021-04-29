using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class SaveNewImageCommand : ICommand
    {
        public UpdateImageVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public SaveNewImageCommand(UpdateImageVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.GettingData = true;
            if(!File.Exists(VM.NewImagePath))
            {
                MessageBox.Show("Debe de seleccionar una imagen valida", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //VM.ImageSource = Path.Combine(VM.imagePath, "no_image.png");
            VM.ImageSource = null;
            VM.NewImageSource = null;
            //VM.NewImageSource = Path.Combine(VM.imagePath, "no_image.png");
            VM.SelectedItem.Image = VM.SelectedItem.Code + ".png";
            Response res = await WebService.InsertData(VM.SelectedItem, URLData.product_updtae_image);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            if (File.Exists(Path.Combine(VM.imagePath, VM.SelectedItem.Image)))
                File.Delete(Path.Combine(VM.imagePath, VM.SelectedItem.Image));
            File.Copy(VM.NewImagePath, Path.Combine(VM.imagePath, VM.SelectedItem.Image), true);

            VM.Clear();
            VM.GettingData = false;
        }
        void CopyPicture()
        {
            try
            {
                
            }
            catch(Exception e) { }
        }
    }
}
