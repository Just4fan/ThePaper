using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class NewsDetail
    {
        public string headLine { get; set; }
        public string about { get; set; }
        public string time { get; set; }
        public string editor { get; set; }
        public List<NewsContent> content { get; set; }

        public NewsDetail()
        {
            content = new List<NewsContent>();
        }
    }

    public class NewsContent
    {
        public const int type_text = 0x0;
        public const int type_video = 0x1;
        public const int type_image = 0x2;
        public object content { get; set; }
        public int type { get; set; }
    }

    public class NewsText
    {
        public string content { get; set; }
        public bool isBold { get; set; }
        public bool isPara { get; set; }
    }

    public class NewsVideo
    {
        public string videoSrc { get; set; }
        public string desc { get; set; }
    }

    public class NewsImage
    {
        public string imageSrc { get; set; }
        public string desc { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public NewsImage()
        {
            imageSrc = desc = string.Empty; 
        }
    }
}
