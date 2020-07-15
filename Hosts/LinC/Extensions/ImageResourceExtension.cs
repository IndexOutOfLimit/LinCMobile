/*using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinC.Extensions
{
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }
        public bool ToFileImageSource { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source is null) return null;

            if (ToFileImageSource)
            {
                return ConvertToFileImageSource();
            }

            ImageSource imageSource = null;

            if (Source.StartsWith("http"))
            {
                imageSource = ImageSource.FromUri(new Uri(Source));
            }
            else
            {
                // Do your translation lookup here, using whatever method you require
                imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);
            }

            return imageSource;
        }

        private ImageSource ConvertToFileImageSource()
        {
            var fileName = Source;
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

            using (var fileStream = File.Create(filePath))
            {
                using (var imgStream = typeof(ImageResourceExtension).GetTypeInfo().Assembly.GetManifestResourceStream(fileName))
                {
                    imgStream?.CopyTo(fileStream);
                }
            }

            return ImageSource.FromFile(filePath);
        }
    }
}
*/