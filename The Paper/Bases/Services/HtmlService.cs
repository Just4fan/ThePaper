using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Bases.Services
{
    public class HtmlService
    {
        public async Task<HtmlDocument> getHtmlDoc(string uri)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(uri);
            return htmlDoc;
        }

        public void Connection(string uri)
        {
            var connection = WebRequest.Create(uri);
            
        }
    }
}
