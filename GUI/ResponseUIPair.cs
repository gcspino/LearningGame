using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace LearningGame.GUI
{
    class ResponseUIPair
    {
        public BitmapImage Image { get; set; }
        public SoundPlayer Sound { get; set; }


        internal ResponseUIPair(string imagePath, string soundPath)
        {
            Image = new BitmapImage();

            Image.BeginInit();
            Image.CacheOption = BitmapCacheOption.OnLoad;
            Image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            Image.UriSource = new Uri(imagePath, UriKind.Absolute);
            Image.EndInit();

            Sound = new SoundPlayer();
            Sound.SoundLocation = soundPath;
        }

        internal void PlaySound()
        {
            if (Sound.SoundLocation != null && File.Exists(Sound.SoundLocation))
            {
                Sound.Play();
            }
        }



    }
}
