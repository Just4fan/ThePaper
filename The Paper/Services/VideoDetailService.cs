using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Bases.Services;
using The_Paper.Models;

namespace The_Paper.Services
{
    public class VideoDetailService : HtmlService
    {
        public async Task<VideoDetail> Load(string uri)
        {
            VideoDetail videoDetail = new VideoDetail();
            var htmlDoc = await getHtmlDoc(uri);
            var videoTxtNode = htmlDoc.DocumentNode.SelectSingleNode(".//div[starts-with(@class,'video_txt_detail')]");
            if(videoTxtNode != null)
            {
                videoDetail.headLine = videoTxtNode.SelectSingleNode("./div[@class='video_txt_t']/h2")?.InnerText;
                videoDetail.mainContent = videoTxtNode.SelectSingleNode("./div[@class='video_txt_l']/p")?.InnerText.Trim();
                videoDetail.time = videoTxtNode.
                    SelectSingleNode("./div[@class='video_txt_l']/div[@class='t_source_1']/div[starts-with(@class,'video_info_first')]/div[@class='video_info_left']/span")?
                    .InnerText;
                videoDetail.source = videoTxtNode.
                    SelectSingleNode("./div[@class='video_txt_l']/div[@class='t_source_1']/div[starts-with(@class,'video_info_first')]/div[@class='video_info_left']/span[@class='oriBox']")?
                    .InnerText;
                videoDetail.commentCount = videoTxtNode.
                    SelectSingleNode("./div[@class='video_txt_l']/div[@class='t_source_1']/div[starts-with(@class,'video_info_first')]/div[@class='video_info_left']/span[@class='reply']")?
                    .InnerText;
                videoDetail.thumbCount = videoTxtNode.
                    SelectSingleNode("./div[@class='video_txt_l']/div[@class='t_source_1']/div[starts-with(@class,'video_info_first')]/div[@class='video_info_right']/div[@class='zanBox']/a")?
                    .InnerText.Trim();
                var infoNodes = videoTxtNode.SelectNodes("./div[@class='video_txt_l']/div[@class='t_source_1']/div[starts-with(@class,'video_info_second')]/span");
                if(infoNodes.Count == 2)
                {
                    videoDetail.about = infoNodes[0].InnerText;
                    videoDetail.editor = infoNodes[1].InnerText;
                }
                var urlNode = htmlDoc.DocumentNode.SelectSingleNode(".//video/source");
                if (urlNode != null)
                    videoDetail.playURL = urlNode.GetAttributeValue("src", string.Empty);
                else
                {
                    string html = htmlDoc.DocumentNode.OuterHtml;
                    string liveURL = string.Empty;
                    int pos = 0;
                    while ((pos = html.IndexOf("<iframe", pos)) != -1)
                    {
                        int end = html.IndexOf('>', pos);
                        string subString = html.Substring(pos, end - pos + 1);
                        var videoNode = HtmlNode.CreateNode(subString);
                        if ((liveURL = videoNode.GetAttributeValue("src", string.Empty)) != string.Empty)
                            break;
                        pos = end;
                    }
                    var liveDocument = await getHtmlDoc(liveURL);
                    string liveString = liveDocument.DocumentNode.OuterHtml;
                    if ((pos = liveString.IndexOf("playFlash(\"")) != -1)
                    {
                        int end = liveString.IndexOf(')', pos);
                        videoDetail.playURL = liveString.Substring(pos + "playFlash(\"".Length,
                            end - pos - "playFlash(\"".Length - 1);
                        //Debug.WriteLine(playURL);
                    }
                }
            }
            return videoDetail;
        }
    }
}
