using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MusicData;

namespace Website.Routes
{
    public class SearchRoute
    {
        public dynamic SearchLibrary(string term)
        {
            var library = Player.Current.Library;

            var result = new
            {
                Songs = library.SearchLibrary(term)

            };

            return JsonConvert.SerializeObject(result);
        }
    }
}
