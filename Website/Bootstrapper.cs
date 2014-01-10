using System.ComponentModel;
using System.IO;
using Nancy;
using MusicData;
using Nancy.Conventions;

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

        protected override IRootPathProvider RootPathProvider
        {
            get { return new CustomRootPathProvider(_websitePath); }
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            AddStaticPath(conventions, "img");
            AddStaticPath(conventions, "lib");
            AddStaticPath(conventions, "css");
            AddStaticPath(conventions, "js");
            AddStaticPath(conventions, "templates");
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

        private void AddStaticPath(NancyConventions conventions, string folderFromRoot)
        {
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory(folderFromRoot, folderFromRoot)
            );
        }
    }
}