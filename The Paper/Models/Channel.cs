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
        public string nodeids { get; set; }
        public string channel_id { get; set; }
        public string update_uri { get; set; }
        public string icon { get; set; }

        public Channel(string name, string uri)
        {
            subChannel = new List<Channel>();
            this.name = name;
            this.uri = uri;
        }

        public Channel(string name, string uri, string nodeids)
        {
            subChannel = new List<Channel>();
            this.name = name;
            this.uri = uri;
            this.nodeids = nodeids;
        }
    }

    public class Column
    {
        public string name { get; set; }
        public string uri { get; set; }
        public string column_id { get; set; }
    }
}
