using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MiejskiDomKultury.Converters
{
    public class ByteArrayToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is byte[] imageData) || imageData.Length == 0)
                return GetDefaultImage();

            try
            {
                using (var stream = new MemoryStream(imageData))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                    return image;
                }
            }
            catch
            {
                return GetDefaultImage();
            }
        }

        private BitmapImage GetDefaultImage()
        {
            // Ścieżka do domyślnego obrazu w projekcie
            var uri = new Uri(Path.Combine(AppContext.BaseDirectory, "imperator.jpg"));
            return new BitmapImage(uri);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}