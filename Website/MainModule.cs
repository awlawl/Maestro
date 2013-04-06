using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Website
{
    public class MainModule : Nancy.NancyModule
    {
        public MainModule()
        {
            Get["/"] = x =>
            {
                return File.ReadAllText(@"..\..\..\Website\Pages\Maestro.html");
            };
        }
    }
}
