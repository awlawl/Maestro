using System.IO;
using Nancy;
using MusicData;

namespace Website
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {

        private byte[] _favicon;
        private string _websitePath;

        public Bootstrapper()
        {
            _websitePath = System.Configuration.ConfigurationManager.AppSettings["WebsitePath"];
        }

       

        protected override byte[] FavIcon
        {
            get { return this._favicon ?? (this._favicon = LoadFavIcon()); }
        }

        private byte[] LoadFavIcon()
        {
            Log.Debug("Loading favorite icon from " + _websitePath);
            return File.ReadAllBytes(_websitePath + "favicon.ico");
        }
    }
}