using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class AddToLibraryController : ApiController
    {

        [HttpPost]
        public void Post(string path)
        {
            var repository = new MemoryLibraryRepository();
            repository.AddDirectoryToLibrary(path);
        }
        
    }
}
