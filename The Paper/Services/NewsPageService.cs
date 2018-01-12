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
using The_Paper.Models;
using Windows.Storage;

namespace The_Paper.Services
{
    public class NewsPageService
    {
        private List<Channel> channelList = ChannelsData.channelList;

        public async Task<NewsListModel> Load(int index, int subIndex)
        {
            NewsListModel newsListModel = new NewsListModel();
            ObservableCollection<News> newsList = new ObservableCollection<News>();
            HtmlWeb web = new HtmlWeb();
            News topNews = new News();
            newsListModel.TopNews = topNews;
            newsListModel.NewsList = newsList;
            newsListModel.NodeIds = ChannelsData.channelList[index].subChannel[subIndex].nodeids;
            newsListModel.UpdateUri = ChannelsData.channelList[index].update_uri;
            var htmlDoc = await web.LoadFromWebAsync(channelList[index].subChannel[subIndex].uri); 
            var ltNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='main_lt']//div[@class='pdtt_lt']");
            if (ltNode != null)
            {
                topNews.image = ltNode.SelectSingleNode(".//img")
                    .GetAttributeValue("src", string.Empty);
                if (!topNews.image.StartsWith("http:"))
                    topNews.image = "http:" + topNews.image;
            }
            var rtNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='main_lt']//div[@class='pdtt_rt']");
            if (rtNode != null)
            {
                var bdNode = rtNode.SelectSingleNode(".//div[@class='pdtt_rtbd']");
                if (bdNode != null)
                {
                    topNews.headLine = bdNode.SelectSingleNode("./h2/a")?.InnerText;
                    topNews.uri = ChannelsData.main + bdNode.SelectSingleNode("./h2/a")?.GetAttributeValue("href", string.Empty);
                    topNews.mainContent = bdNode.SelectSingleNode("./p").InnerText;
                    var trbsNode = rtNode.SelectSingleNode(".//div[@class='pdtt_trbs']");
                    if (trbsNode != null)
                    {
                        topNews.tag = trbsNode.SelectSingleNode("./a")?.InnerText;
                        topNews.time = trbsNode.SelectSingleNode("./span")?.InnerText;
                        var commentCount = trbsNode.SelectSingleNode("./span[@class='trbszan']");
                        if (commentCount != null)
                            topNews.commentCount = commentCount.InnerText;
                    }
                }
            }
            parseNewsList(newsListModel, htmlDoc);
            return newsListModel;
        }

        public async Task LoadMore(NewsListModel newsListModel)
        {
            string uri = string.Format("{0}nodeids={1}&topCids={2}&pageIndex={3}&lastTime={4}"
                ,newsListModel.UpdateUri
                ,newsListModel.NodeIds
                ,newsListModel.TopCids
                ,newsListModel.PageIndex + 1
                ,newsListModel.LastTime);
            //Debug.WriteLine(uri);
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(uri);
            parseNewsList(newsListModel, htmlDoc);
        }

        public void parseNewsList(NewsListModel newsListModel, HtmlDocument htmlDoc)
        {
            var newsNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='newsbox']/div[@class='news_li']");
            if (newsNodes == null)
            {
                newsNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='news_li'] | //div[@lasttime]");
                if (newsNodes == null)
                    return;
            }
            foreach (var newsNode in newsNodes)
            {
                string lastTime = string.Empty;
                if ((lastTime = newsNode.GetAttributeValue("lastTime", string.Empty)) != string.Empty)
                {
                    newsListModel.LastTime = lastTime;
                    int pageIndex;
                    if ((pageIndex = newsNode.GetAttributeValue("pageIndex", -1)) != -1)
                        newsListModel.PageIndex = pageIndex;
                    //Debug.WriteLine(newsListModel.PageIndex);
                    continue;
                }
                News news = new News();
                var news_tu = newsNode.SelectSingleNode(".//div[@class='news_tu']/a");
                if (news_tu != null)
                {
                    news.cid = news_tu.GetAttributeValue("data-id", string.Empty);
                    news.uri = ChannelsData.main + news_tu.GetAttributeValue("href", string.Empty);
                    news.image = news_tu.SelectSingleNode("./img").GetAttributeValue("src", string.Empty);
                    if (!news.image.StartsWith("http:"))
                        news.image = "http:" + news.image;
                }
                news.headLine = newsNode.SelectSingleNode(".//h2").SelectSingleNode(".//a").InnerText;
                news.mainContent = newsNode.SelectSingleNode(".//p").InnerText.Trim();
                var pdtt = newsNode.SelectSingleNode(".//div[@class='pdtt_trbs']");
                if (pdtt != null)
                {
                    news.tag = pdtt.SelectSingleNode("./a").InnerText;
                    var commentCount = pdtt.SelectSingleNode("./span[@class='trbszan']");
                    if (commentCount != null)
                        news.commentCount = commentCount.InnerText;
                    news.time = pdtt.SelectSingleNode("./span")?.InnerText;
                    var isRecommend = pdtt.SelectSingleNode("./div[@class='trbstxt']");
                    if (isRecommend != null)
                        newsListModel.TopCids += news.cid + ',';
                }
                newsListModel.NewsList.Add(news);
            }
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
