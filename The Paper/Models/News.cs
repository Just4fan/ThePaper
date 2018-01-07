using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class News
    {
        public string uri { get; set; }
        public string cid { get; set; }
        public string image { get; set; }
        public string headLine { get; set; }
        public string mainContent { get; set; }
        public string commentCount { get; set; }
        public string time { get; set; }
        public string tag { get; set; }
    }
}
