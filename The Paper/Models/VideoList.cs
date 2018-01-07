using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class VideoList
    {
        public Video topVideo { get; set; }
        public ObservableCollection<Video> videoList { get; set; }
        public string updateUri { get; set; }
        public string channelID { get; set; }
        public int pageIndex { get; set; }

        public VideoList()
        {
            videoList = new ObservableCollection<Video>();
        }
    }
}
