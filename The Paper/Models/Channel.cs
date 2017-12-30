using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class Channel
    {
        public List<Channel> subChannel;
        public string name { get; set; }
        public string uri { get; set; }

        public Channel(string name, string uri)
        {
            subChannel = new List<Channel>();
            this.name = name;
            this.uri = uri;
        }
    }
}
