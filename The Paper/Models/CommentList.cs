using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class CommentList
    {
        public ObservableCollection<Comment> HotComments { get; set; }
        public ObservableCollection<Comment> NewsComments { get; set; }

        public CommentList()
        {
            HotComments = new ObservableCollection<Comment>();
            NewsComments = new ObservableCollection<Comment>();
        }
    }
}
