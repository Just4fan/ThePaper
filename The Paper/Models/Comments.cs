using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class Comments
    {
        public string contid { get; set; }
        public string hotIds { get; set; }
        public int pageidx { get; set; }
        public string startId { get; set; }
        public string loadURL { get; set; }
        public string updateURL { get; set; }
        public ObservableCollection<Comment> hotComments { get; set; }
        public ObservableCollection<Comment> newComments { get; set; }

        public Comments()
        {
            hotComments = new ObservableCollection<Comment>();
            newComments = new ObservableCollection<Comment>();
        }
    }
}
