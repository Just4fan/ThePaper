using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using The_Paper.Bases.Services;
using The_Paper.Data;
using The_Paper.Models;

namespace The_Paper.Services
{
    public class VideoPageService : HtmlService
    {
        public VideoPageService()
        {

        }

        public async Task<VideoList> Load(Channel channel, int columnIdx)
        {
            VideoList videoList = new VideoList();
            videoList.updateUri = channel.update_uri;
            videoList.channelID = channel.columns[columnIdx].column_id;
            videoList.pageIndex = 1;
            string uri = channel.columns[columnIdx].uri;
            HtmlDocument htmlDoc = null;
            if (columnIdx == 0)
                htmlDoc = await getHtmlDoc(uri);
            else
            {
                videoList.pageIndex = 0;
                htmlDoc = await getHtmlDoc(string.Format("{0}nodeid={1}&channelID={2}&_={3}",
                    "http://www.thepaper.cn/video_list_masonry.jsp?",
                    videoList.channelID,
                    channel.channel_id,
                    (long)((DateTime.Now - new DateTime(1970,1,1)).TotalMilliseconds)));
            }
            var topVideoNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='slide_container']");
            Video topVideo = new Video();
            if(topVideoNode != null)
            {
                string imgSrc = topVideoNode.GetAttributeValue("style", string.Empty);
                int index1 = imgSrc.IndexOf('(');
                int index2 = imgSrc.IndexOf(')');
                if (index1 != -1 && index2 != -1)
                {
                    topVideo.imageSrc = imgSrc.Substring(index1 + 1, index2 - index1 - 1);
                    if (!topVideo.imageSrc.StartsWith("http:"))
                        topVideo.imageSrc = "http:" + topVideo.imageSrc;
                }
                //Debug.WriteLine(topVideo.imageSrc);
                topVideo.headLine = topVideoNode.SelectSingleNode(".//div[@class='slide_title']")?.InnerText;
                topVideo.length = topVideoNode.SelectSingleNode(".//div[@class='slide_time']")?.InnerText;
                //Debug.WriteLine("headLine:" + topVideo.headLine);
                //Debug.WriteLine("lenght:" + topVideo.length);
                var SourceL = topVideoNode.SelectSingleNode(".//div[@class='t_source_1']");
                if(SourceL != null)
                {
                    topVideo.tag = SourceL.SelectSingleNode(".//span[@class='source_bk']")?.InnerText;
                    topVideo.time = SourceL.SelectNodes(".//span")?[1].InnerText;
                    topVideo.type = SourceL.SelectSingleNode(".//div[@class='video_top_corner']")?.InnerText;
                }
                topVideo.uri = ChannelsData.main 
                    + htmlDoc.DocumentNode.SelectSingleNode(".//div[@class='video_txt_l']/p/a")?
                    .GetAttributeValue("href", string.Empty);
                //Debug.WriteLine("uri:" + topVideo.uri);
                //Debug.WriteLine("tag:" + topVideo.tag);
                //Debug.WriteLine("time:" + topVideo.time);
                //Debug.WriteLine("type:" + topVideo.type);
            }
            ParseVideoList(htmlDoc, videoList);
            videoList.topVideo = topVideo;
            return videoList;
        }

        public void ParseVideoList(HtmlDocument htmlDoc, VideoList videoList)
        {
            var videoListNodes = htmlDoc.DocumentNode.SelectNodes("//li[@class='video_news']");
            if (videoListNodes != null)
            {
                foreach (var videoNode in videoListNodes)
                {
                    Video video = new Video();
                    video.uri = ChannelsData.main + videoNode.SelectSingleNode("./div[@class='video_list_pic']/a")?
                        .GetAttributeValue("href", string.Empty);
                    video.length = videoNode.SelectSingleNode("./div[@class='video_list_pic']/span[@class='p_time']")?
                        .InnerText;
                    video.imageSrc = videoNode.SelectSingleNode("./div[@class='video_list_pic']/img")?
                        .GetAttributeValue("src", string.Empty);
                    if (!video.imageSrc.StartsWith("http:"))
                        video.imageSrc = "http:" + video.imageSrc;
                    video.headLine = videoNode.SelectSingleNode(".//div[@class='video_title']")?
                        .InnerText.TrimStart();
                    if (video.headLine == null)
                        video.headLine = videoNode.SelectSingleNode(".//h2[@class='video_title']")?
                            .InnerText.TrimStart();
                    video.mainContent = videoNode.SelectSingleNode(".//p")?
                        .InnerText.TrimStart();
                    var tSource = videoNode.SelectSingleNode("./div[@class='t_source']");
                    if (tSource != null)
                    {
                        video.tag = tSource.SelectSingleNode("./a")?.InnerText;
                        video.time = tSource.SelectNodes("./span")?[0].InnerText;
                        video.commentCount = tSource.SelectSingleNode("./span[@class='reply']")?.InnerText;
                        string cid = string.Empty;
                    }
                    videoList.videoList.Add(video);
                }
            }
        }

        public async Task<bool> LoadMore(VideoList videoList)
        {
            int count = videoList.videoList.Count;
            var htmlDoc = await getHtmlDoc(APIService.apiService.GetURL(videoList));
            ParseVideoList(htmlDoc, videoList);
            if (videoList.videoList.Count != count)
                return true;
            return false;
        }
    }
}
