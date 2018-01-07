using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class Video
    {
        public const int TYPE_LIVE = 0x0;
        public const int TYPE_VIDEO = 0x1;

        public string type { get; set; }
        public string imageSrc { get; set; }
        public string tag { get; set; }
        public string headLine { get; set; }
        public string length { get; set; }
        public string mainContent { get; set; }
        public string time { get; set; }
        public string commentCount { get; set; }
        public string live { get; set; }
        public string uri { get; set; }

        public Video()
        {
            
        }
    }
}
