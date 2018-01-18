using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Models;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace The_Paper.Data
{
    public class ChannelsData
    {
        public static string main = @"http://www.thepaper.cn/";
        public const string commentURL = @"http://www.thepaper.cn/newDetail_commt.jsp?";
        public static List<Channel> Channels = new List<Channel>();

        public static async Task InitAsync()
        {
            StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Channels.xml"));
            Stream stream = await storageFile.OpenStreamForReadAsync();
            XDocument xDocument = XDocument.Load(stream);
            var channels = xDocument.Element("Channels").Elements();
            foreach(var element in channels)
            {
                Channel channel = new Channel();
                channel.name = element.Element(XName.Get("Channel-Name"))?.Value;
                channel.uri = element.Element(XName.Get("Channel-Url"))?.Value;
                channel.icon = element.Element(XName.Get("Channel-Icon"))?
                    .Element(XName.Get("Icon-Text"))?.Value;
                channel.channel_id = element.Element(XName.Get("Channel-Id"))?.Value;
                channel.update_uri = element.Element(XName.Get("Channel-More"))?
                    .Element(XName.Get("Url")).Value;
                var columns = element.Element(XName.Get("Channel-Colums"))?.Elements();
                foreach(var columnEle in columns)
                {
                    Column column = new Column();
                    column.column_id = columnEle.Element("Column-Id")?.Value;
                    column.name = columnEle.Element("Column-Name")?.Value;
                    column.uri = columnEle.Element("Column-Url")?.Value;
                    channel.columns.Add(column);
                }
                Channels.Add(channel);
            }
        }

        /*
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
                string nodeids = string.Empty;
                if (name.Equals("精选"))
                {
                    nodeids = "25949";
                    channel.update_uri = main + "load_chosen.jsp?";
                }
                else
                    channel.update_uri = main + "load_index.jsp?";
                if (ltNodes != null)
                    foreach(var ltNode in ltNodes)
                    {
                        string ltName = ltNode.SelectSingleNode("a").
                            InnerText.Trim();
                        string ltUri = ltNode.SelectSingleNode("a").
                            GetAttributeValue("href", string.Empty);
                        string ltNodeid = ltUri.TrimStart("list_".ToArray());
                        ltUri = main + ltUri;
                        channel.subChannel.Add(
                            new Channel(ltName, ltUri, ltNodeid));
                        nodeids += ltNodeid + ',';
                        //Debug.WriteLine("sub name:" + ltName + " uri:" + ltUri);
                    }
                //Debug.WriteLine(nodeids);
                channel.subChannel[0].nodeids = nodeids;
                channelList.Add(channel);
            }
            StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Channels.xml"));
            Stream stream = await storageFile.OpenStreamForReadAsync();
            XDocument xDocument = XDocument.Load(stream);
            var collection = xDocument.Element("Channels").Elements();
            for (int i = 0; i < collection.Count(); i++)
            {
                XElement columns = new XElement("Channel-Colums");
                foreach(Channel sub in channelList[i].subChannel)
                {
                    XElement column = new XElement("Column");
                    XElement ColumnName = new XElement("Column-Name");
                    ColumnName.SetValue(sub.name);
                    XElement ColumnUrl = new XElement("Column-Url");
                    ColumnUrl.SetValue(sub.uri);
                    column.Add(ColumnName, ColumnUrl);
                    columns.Add(column);
                }
                channelList[i].icon = (string)collection.ElementAt(i).Element("Channel-Icon").Element("Icon-Text");
            }
            return true;
        }
        */
    }
}
