

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LearningGame.GUI
{
    public class ImageResources
    {
        Dictionary<string, BitmapImage> mResources;
        Random rnd = new Random();
        const string IMAGE_EXT = ".png";

        public ImageResources(string initPath)
        {
            mResources = new Dictionary<string, BitmapImage>();

            List<ResponseUIPair> Responses = new List<ResponseUIPair>();

            List<string> files = Directory.EnumerateFiles(initPath)
                .Where(c => c.ToLower().Contains(IMAGE_EXT))
                .Select(f => f.ToLower())
                .ToList();

            foreach (string file in files)
            {
                BitmapImage Image = new BitmapImage();

                Image.BeginInit();
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                Image.UriSource = new Uri(file, UriKind.Absolute);
                Image.EndInit();

                mResources.Add(Path.GetFileName(file), Image);
            }         
        }

        internal BitmapImage GetImage(string resourceName)
        {
            string resourceLower = resourceName.ToLower();

            if (mResources.ContainsKey(resourceLower))
            {
                    return mResources[resourceLower];
            }

            return null;

        }

    }
}
