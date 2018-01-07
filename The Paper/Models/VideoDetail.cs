using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class VideoDetail
    {
        public const int TYPE_VIDEO = 0x0;
        public const int TYPE_LIVE = 0x1;

        public string headLine { get; set; }
        public string mainContent { get; set; }
        public string thumbCount { get; set; }
        public string commentCount { get; set; }
        public string time { get; set; }
        public string source { get; set; }
        public string editor { get; set; }
        public string about { get; set; }
        public string playURL { get; set; }
        public int type { get; set; }
    }
}
