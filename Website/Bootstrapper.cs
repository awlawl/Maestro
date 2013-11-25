using System.IO;
using Nancy;

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
            return File.ReadAllBytes(_websitePath + "favicon.ico");
        }
    }
}