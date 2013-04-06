using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using System.IO;

namespace Website
{
    public class ImageResponse : Response
    {
        public ImageResponse(byte[] data, string contentType)
        {
            this.Contents = GetImageStream(data);
            this.ContentType = contentType;
            this.StatusCode = HttpStatusCode.OK;
        }


        Action<Stream> GetImageStream(byte[] data)
        {
            return stream =>
            {
                var x = new BinaryWriter(stream);
                x.Write(data);
                x.Close();
            };
        }
    }
}
