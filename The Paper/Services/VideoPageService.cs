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

        public async Task<VideoList> Load(int index, int subindex)
        {
            VideoList videoList = new VideoList();
            string uri = ChannelsData.channelList[index].subChannel[subindex].uri;
            var htmlDoc = await getHtmlDoc(uri);
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
            var videoListNodes = htmlDoc.DocumentNode.SelectNodes("//li[@class='video_news']");
            if(videoListNodes != null)
            {
                foreach(var videoNode in videoListNodes)
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
                    video.mainContent = videoNode.SelectSingleNode(".//p")?
                        .InnerText.TrimStart();
                    var tSource = videoNode.SelectSingleNode("./div[@class='t_source']");
                    if(tSource != null)
                    {
                        video.tag = tSource.SelectSingleNode("./a")?.InnerText;
                        video.time = tSource.SelectNodes("./span")?[0].InnerText;
                        video.commentCount = tSource.SelectSingleNode("./span[@class='reply']")?.InnerText;
                        string cid = string.Empty;
                    }
                   // Debug.WriteLine("commentCount:" + video.commentCount);
                    //Debug.WriteLine("headLine:" + video.headLine);
                    //Debug.WriteLine("mainContent:" + video.mainContent);
                    //Debug.WriteLine("time:" + video.time);
                    //Debug.WriteLine("uri:" + video.uri);
                    //Debug.WriteLine("imageSrc:" + video.imageSrc);
                    //Debug.WriteLine("tag:" + video.tag);
                    //Debug.WriteLine("length:" + video.length);
                    videoList.videoList.Add(video);
                }
            }
            videoList.topVideo = topVideo;
            return videoList;
        }

        public void LoadMore(VideoList videoList)
        {
            
        }
    }
}
