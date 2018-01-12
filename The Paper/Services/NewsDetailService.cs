using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Bases.Services;
using The_Paper.Models;

namespace The_Paper.Services
{
    public class NewsDetailService : HtmlService
    {
        public async Task<NewsDetail> Load(string uri)
        {
            var htmlDoc = await getHtmlDoc(uri);
            NewsDetail newsDetail = new NewsDetail();
            var newsContent = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='newscontent']");
            int index = uri.LastIndexOf("_");
            if(index != -1)
                newsDetail.id = uri.Substring(index + 1);
            foreach(var node in newsContent.ChildNodes)
            {
                if (node.GetAttributeValue("class", string.Empty).Equals("news_title"))
                    newsDetail.headLine = node.InnerText;
                else if(node.GetAttributeValue("class", string.Empty).Equals("news_about"))
                {
                    var newsAbout = node.SelectNodes("./p");
                    if(newsAbout != null && newsAbout.Count == 2)
                    {
                        newsDetail.about = newsAbout[0].FirstChild.InnerText;
                        newsDetail.time = newsAbout[1].FirstChild.InnerText.Trim();
                    }
                }
                else if(node.GetAttributeValue("class", string.Empty).Equals("news_video_name"))
                {
                    NewsContent item = new NewsContent();
                    NewsVideo video = new NewsVideo();
                    item.type = NewsContent.type_video;
                    string html = htmlDoc.DocumentNode.OuterHtml;
                    int pos = html.IndexOf(".mp4");
                    if(pos != -1)
                    {
                        int pos0 = pos;
                        while (html[pos0] != '\'') --pos0;
                        video.videoSrc = html.Substring(pos0 + 1, pos - pos0 + 3);
                    }
                    video.desc = node.LastChild.NodeType == HtmlNodeType.Text ?
                        node.LastChild.InnerText : string.Empty;
                    item.content = video;
                    newsDetail.content.Add(item);
                }
                else if(node.GetAttributeValue("class", string.Empty).StartsWith("news_txt"))
                {
                    foreach(var txtNode in node.ChildNodes)
                    {
                        if(txtNode.NodeType == HtmlNodeType.Text && txtNode.InnerText.Length > 0)
                        {
                            NewsContent content = new NewsContent();
                            NewsText newsText = new NewsText();
                            newsText.content = txtNode.InnerText;
                            var nextNode = txtNode.NextSibling;
                            if (nextNode != null && nextNode.Name.Equals("br"))
                                newsText.isPara = true;
                            content.type = NewsContent.type_text;
                            content.content = newsText;
                            newsDetail.content.Add(content);
                        }
                        else if(txtNode.NodeType == HtmlNodeType.Element &&
                            txtNode.GetAttributeValue("style", string.Empty).Equals("text-align:center;"))
                        {
                            NewsContent content = new NewsContent();
                            NewsImage newsImage = new NewsImage();
                            newsImage.imageSrc = txtNode.SelectSingleNode(".//img")?
                                .GetAttributeValue("src", string.Empty);
                            newsImage.Width = double.Parse(txtNode.SelectSingleNode(".//img")?
                                .GetAttributeValue("width", "0"));
                            newsImage.Height = double.Parse(txtNode.SelectSingleNode(".//img")?
                                .GetAttributeValue("height", "0"));
                            content.content = newsImage;
                            content.type = NewsContent.type_image;
                            if (txtNode.NextSibling != null && txtNode.NextSibling.Name.Equals("span"))
                                newsImage.desc = txtNode.NextSibling.InnerText;
                            newsDetail.content.Add(content);
                        }
                        else if(txtNode.NodeType == HtmlNodeType.Element && txtNode.Name.Equals("strong"))
                        {
                            NewsContent content = new NewsContent();
                            NewsText newsText = new NewsText();
                            newsText.content = txtNode.InnerText;
                            var nextNode = txtNode.NextSibling;
                            if (nextNode.Name.Equals("br"))
                                newsText.isPara = true;
                            newsText.isBold = true;
                            content.type = NewsContent.type_text;
                            content.content = newsText;
                            newsDetail.content.Add(content);
                        }
                    }
                }
                else if(node.GetAttributeValue("class", string.Empty).StartsWith("news_editor"))
                {
                    newsDetail.editor = node.FirstChild.InnerText;
                }
            }
            return newsDetail;
        }

        private void Recursive(HtmlNode txtNode)
        {

        }
    }
}
