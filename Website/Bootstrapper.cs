using System.IO;
using Nancy;

namespace Website
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private byte[] favicon;

        protected override byte[] FavIcon
        {
            get { return this.favicon ?? (this.favicon = LoadFavIcon()); }
        }

        private byte[] LoadFavIcon()
        {
            return File.ReadAllBytes(@"..\..\..\Website\Pages\favicon.ico");
        }
    }
}