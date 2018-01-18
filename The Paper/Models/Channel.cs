using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class Channel
    {
        public List<Column> columns;
        public string name { get; set; }
        public string uri { get; set; }
        public string nodeids { get; set; }
        public string channel_id { get; set; }
        public string update_uri { get; set; }
        public string icon { get; set; }

        public Channel(string name, string uri)
        {
            columns = new List<Column>();
            this.name = name;
            this.uri = uri;
        }

        public Channel(string name, string uri, string nodeids)
        {
            columns = new List<Column>();
            this.name = name;
            this.uri = uri;
            this.nodeids = nodeids;
        }

        public Channel()
        {
            columns = new List<Column>();
        }
    }

    public class Column
    {
        public string name { get; set; }
        public string uri { get; set; }
        public string column_id { get; set; }
    }
}
