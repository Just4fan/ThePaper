using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Windows.Storage;
using System.Diagnostics;

namespace The_Paper.Http
{
    public class HttpRequest
    {
        public static async Task<HttpWebResponse> doGet(string url, string param)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return response;
        }
    }
}
