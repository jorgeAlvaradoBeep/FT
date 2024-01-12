using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Facturacion_Tostatronic.ViewModels.Products
{
    public class UpdateImageVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "UpdateImageVM";
        public string imagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string imagePathBase = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        BitmapImage oldImage, newImage;
        public SearchProductsForUpdateImagecommand SearchProductsForUpdateImagecommand { get; set; }
        public GetProductsWNImageCommand GetProductsWNImageCommand { get; set; }
        public GetProductsWCImageCommand GetProductsWCImageCommand { get; set; }
        public SaveNewImageCommand SaveNewImageCommand { get; set; }
        private string cs;

        public string CS
        {
            get { return cs; }
            set { SetValue(ref cs, value); }
        }
        private List<ProductBase> searchProducts;

        public List<ProductBase> SearchProducts
        {
            get { return searchProducts; }
            set { SetValue(ref searchProducts, value); }
        }

        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set { SetValue(ref gettingData, value); }
        }
        private BitmapImage imageSource;

        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set { SetValue(ref imageSource, value); }
        }

        private BitmapImage newImageSource;
        public BitmapImage NewImageSource
        {
            get { return newImageSource; }
            set { SetValue(ref newImageSource, value); }
        }


        private ProductBase selectedItem;

        public ProductBase SelectedItem
        {
            get { return selectedItem; }
            set 
            { 
                if(value!=null)
                {
                    if (string.IsNullOrEmpty(value.Image))
                        return;
                    EnableProductBase = true;
                    value.Image = Path.Combine(imagePath, value.Image);
                    if (File.Exists(value.Image))
                    {
                        try 
                        {
                            //File.Copy(value.Image, Path.Combine(imagePathBase + @"\Tostatronic\temp.png"), true);
                            //string auxImage = Path.Combine(imagePathBase + @"\Tostatronic\temp.png");
                            oldImage = CreateSource(value.Image);
                            ImageSource = oldImage;
                        }
                        catch(Exception ex) 
                        {
                            MessageBox.Show($"Error al copiar imagen.{Environment.NewLine}" +
                                $"Motivo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        
                    }
                    else
                    {
                        oldImage = CreateSource(Path.Combine(imagePath, "no_image"));
                        //ImageSource = Path.Combine(imagePathBase + @"\Tostatronic\temp.png");
                        ImageSource = oldImage;
                    }
                }
                SetValue(ref selectedItem, value);

            }
        }

        private bool enableProductBase;

        public bool EnableProductBase
        {
            get { return enableProductBase; }
            set { SetValue(ref enableProductBase, value); }
        }

        private string newImagePath;

        public string NewImagePath
        {
            get { return newImagePath; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    SetValue(ref newImagePath, value);
                    return;
                }
                    
                if (Path.GetExtension(value).Equals(".png"))
                {
                    SetValue(ref newImagePath, value);
                    //File.Copy(newImagePath, Path.Combine(imagePathBase + @"\Tostatronic\tempN.png"), true);
                    newImage = CreateSource(NewImagePath);
                    NewImageSource = newImage;
                }
                else
                {
                    MessageBox.Show("Formato no compatible", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                    
                 
            }
        }
        




        public UpdateImageVM()
        {
            SearchProductsForUpdateImagecommand = new SearchProductsForUpdateImagecommand(this);
            GetProductsWNImageCommand = new GetProductsWNImageCommand(this);
            GetProductsWCImageCommand = new GetProductsWCImageCommand(this);
            SaveNewImageCommand = new SaveNewImageCommand(this);
            imagePath = Path.Combine(imagePath, @"MEGAsync\Imagenes\");
            EnableProductBase = false;
        }

        public void Clear()
        {
            SelectedItem = new ProductBase();
            NewImagePath = null;
            NewImageSource = null;
            ImageSource = null;
            EnableProductBase = false;
        }

        BitmapImage CreateSource (string path)
        {
            //BitmapImage bitmap = new BitmapImage();
            //bitmap.BeginInit();
            //bitmap.UriSource = new Uri(path);
            //bitmap.CacheOption = BitmapCacheOption.OnLoad;
            //bitmap.EndInit();
            //return bitmap;


            BitmapImage albumArt = new BitmapImage();  // Create new BitmapImage  
            Stream stream = new MemoryStream();  // Create new MemoryStream  
            Bitmap bitmap = new Bitmap(path);  // Create new Bitmap (System.Drawing.Bitmap) from the existing image file (albumArtSource set to its path name)  
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);  // Save the loaded Bitmap into the MemoryStream - Png format was the only one I tried that didn't cause an error (tried Jpg, Bmp, MemoryBmp)  
            bitmap.Dispose();  // Dispose bitmap so it releases the source image file  
            albumArt.BeginInit();  // Begin the BitmapImage's initialisation  
            albumArt.StreamSource = stream;  // Set the BitmapImage's StreamSource to the MemoryStream containing the image  
            albumArt.EndInit();  // End the BitmapImage's initialisation 
            return albumArt;
        }

    }
}
