using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Data;
using The_Paper.Http;
using The_Paper.Models;
using Windows.Storage;

namespace The_Paper.Services
{
    public class NewsPageService
    {
        private List<Channel> channelList = ChannelsData.channelList;

        public async Task<NewsListModel> parseHtml(int index, int subIndex)
        {
            NewsListModel newsListModel = new NewsListModel();
            ObservableCollection<News> newsList = new ObservableCollection<News>();
            HtmlWeb web = new HtmlWeb();
            News topNews = new News();
            //Debug.WriteLine(channelList[index].subChannel[subIndex].uri);
            var htmlDoc = await web.LoadFromWebAsync(channelList[index].subChannel[subIndex].uri); 
            var ltNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='main_lt']//div[@class='pdtt_lt']");
            if(ltNode != null)
                topNews.image = ltNode.SelectSingleNode(".//img")
                    .GetAttributeValue("src", string.Empty);
            var rtNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='main_lt']//div[@class='pdtt_rt']");
            if (rtNode != null)
            {
                var bdNode = rtNode.SelectSingleNode(".//div[@class='pdtt_rtbd']");
                if (bdNode != null)
                {
                    topNews.headLine = bdNode.SelectSingleNode("./h2/a").InnerText;
                    topNews.mainContent = bdNode.SelectSingleNode("./p").InnerText;
                    var trbsNode = bdNode.SelectSingleNode(".//div[@class='pdtt_trbs']");
                    if (trbsNode != null)
                    {
                        topNews.tag = trbsNode.SelectSingleNode("./a").InnerText;
                        var commentCount = trbsNode.SelectSingleNode("./span[@class='trbszan']");
                        if (commentCount != null)
                            topNews.commentCount = commentCount.InnerText;
                    }
                }
            }
            
            var newsNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='news_li']");
            if (newsNodes == null)
                return newsListModel;
            foreach(var newsNode in newsNodes)
            {
                if (newsNode.GetAttributeValue("pageindex", -1) != -1)
                    continue;
                News news = new News();
                news.image = newsNode.SelectSingleNode(".//img").GetAttributeValue("src", string.Empty);
                news.headLine = newsNode.SelectSingleNode(".//h2").SelectSingleNode(".//a").InnerText;
                news.mainContent = newsNode.SelectSingleNode(".//p").InnerText.Trim();
                var pdtt = newsNode.SelectSingleNode(".//div[@class='pdtt_trbs']");
                if (pdtt != null)
                {
                    news.tag = pdtt.SelectSingleNode("./a").InnerText;
                    var commentCount = pdtt.SelectSingleNode("./span[@class='trbszan']");
                    if (commentCount != null)
                        news.commentCount = commentCount.InnerText;
                }
                newsList.Add(news);
            }
            newsListModel.TopNews = topNews;
            newsListModel.NewsList = newsList;
            return newsListModel;
        }

        public async void WriteFileFromStream(Stream stream, StorageFolder folder, string filename)
        {
            StorageFolder cacheFolder = ApplicationData.Current.LocalCacheFolder;
            Debug.WriteLine(cacheFolder.Path);
            StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            Stream ws = await file.OpenStreamForWriteAsync();
            byte[] buffer = new byte[4096];
            int count;
            while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                ws.Write(buffer, 0, count);
            ws.Dispose();
        }
    }
}
