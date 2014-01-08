using Nancy;

namespace Website
{
    public class CustomRootPathProvider : IRootPathProvider
    {
        public string RootPath { get; set; }

        public CustomRootPathProvider(string rootPath)
        {
            RootPath = rootPath;
        }

        public string GetRootPath()
        {
            return RootPath;
        }
    }
}
