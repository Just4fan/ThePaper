using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class VideoContent
    {
        public string playURL { get; set; }
        public string headLine { get; set; }
        public string mainContent { get; set; }
        public ObservableCollection<Comment> commentList { get; set; }
    }
}
