using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Models;

namespace The_Paper.Data
{
    public class ChannelsData
    {
        private static string main = @"http://www.thepaper.cn/";
        public static List<Channel> channelList = new List<Channel>();

        public static async Task<bool> generateList()
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(main);
            var nodes = htmlDoc.DocumentNode.
                SelectNodes("//div[starts-with(@class,'bn_bt')]");
            if (nodes == null)
                return true;
            foreach(HtmlNode node in nodes)
            {
                string name = node.SelectSingleNode("a").InnerText;
                string uri = main + 
                    node.SelectSingleNode("a").
                    GetAttributeValue("href", string.Empty);
                Channel channel = new Channel(name, uri);
                channel.subChannel.Add(new Channel("推荐", uri));
                //Debug.WriteLine("name:" + name + " uri:" + uri);
                var ltNodes = node.SelectNodes(".//li");
                if (ltNodes != null)
                    foreach(var ltNode in ltNodes)
                    {
                        string ltName = ltNode.SelectSingleNode("a").
                            InnerText.Trim();
                        string ltUri = main + ltNode.SelectSingleNode("a").
                            GetAttributeValue("href", string.Empty);
                        channel.subChannel.Add(
                            new Channel(ltName, ltUri));
                        //Debug.WriteLine("sub name:" + ltName + " uri:" + ltUri);
                    }
                channelList.Add(channel);
            }
            return true;
        }
    }
}
