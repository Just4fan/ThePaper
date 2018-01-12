using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class Comment
    {
        public string avatarSrc { get; set; }
        public string content { get; set; }
        public string time { get; set; }
        public string thumbCount { get; set; }
        public string userHome { get; set; }
        public string userName { get; set; }
        public string commentID { get; set; }
        public List<CommentFloor> commentFloors { get; set; }

        public Comment()
        {
            commentFloors = new List<CommentFloor>();
        }
    }

    public class CommentFloor
    {
        public string userName { get; set; }
        public string userHome { get; set; }
        public string content { get; set; }
        public string thumbCount { get; set; }
    }
}
