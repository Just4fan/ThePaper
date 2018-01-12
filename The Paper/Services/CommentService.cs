using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Bases.Services;
using The_Paper.Models;

namespace The_Paper.Services
{
    public class CommentService : HtmlService
    {
        private HtmlDocument htmlDoc;

        public async Task InitAsync(string uri)
        {
            htmlDoc = await getHtmlDoc(uri);
        }

        public ObservableCollection<Comment> LoadHot()
        {
            ObservableCollection<Comment> commentList = new ObservableCollection<Comment>();
            var commentTitles = htmlDoc.DocumentNode.SelectNodes(".//div[@class='comment_title']");
            if(commentTitles != null)
            {
                foreach(var commentTitle in commentTitles)
                {
                    var comment = commentTitle.NextSibling;
                    while (comment != null)
                    {
                        string attr = comment.GetAttributeValue("class", string.Empty);
                        if (attr.Equals("comment_title"))
                            break;
                        if (attr.Equals("comment_que"))
                            commentList.Add(GetComment(comment));
                        comment = comment.NextSibling;
                    }
                }
            }
            return commentList;
        }

        public ObservableCollection<Comment> LoadNew()
        {
            ObservableCollection<Comment> commentList = new ObservableCollection<Comment>();
            var commentTitles = htmlDoc.DocumentNode.SelectNodes(".//div[@class='comment_title']");
            if(commentTitles != null)
                foreach(var commentTitle in commentTitles)
                {
                    if(commentTitle.InnerText.Equals("新评论"))
                    {
                        var comment = commentTitle.NextSibling;
                        while (comment != null)
                        {
                            string attr = comment.GetAttributeValue("class", string.Empty);
                            if (attr.Equals("comment_que"))
                                commentList.Add(GetComment(comment));
                            comment = comment.NextSibling;
                        }
                    }
                }
            return commentList;
        }

        private Comment GetComment(HtmlNode node)
        {
            Comment comment = new Comment();
            //var commentNode = node.SelectSingleNode(".//div[@class='comment_que']");
            comment.commentID = node.GetAttributeValue("id", string.Empty);
            var left = node.SelectSingleNode(".//div[@class='aqwleft']");
            if(left != null)
            {
                comment.userHome = left.SelectSingleNode(".//a")?.GetAttributeValue("href", string.Empty);
                comment.avatarSrc = left.SelectSingleNode(".//a/img")?.GetAttributeValue("src", string.Empty).TrimStart('/');
                if (!comment.avatarSrc.StartsWith("http:"))
                    comment.avatarSrc = "http://" + comment.avatarSrc;
            }
            var right = node.SelectSingleNode(".//div[@class='aqwright']");
            if(right != null)
            {
                comment.userName = right.SelectSingleNode("./h3/a")?.InnerText;
                comment.time = right.SelectSingleNode("./h3/span")?.InnerText;
                var floors = right.SelectNodes("./div[@class='floor_content']/div[@class='ansright_contans']");
                if(floors != null)
                {
                    foreach(var floor in floors)
                    {
                        CommentFloor commentFloor = new CommentFloor();
                        commentFloor.thumbCount = floor.SelectSingleNode("./div[@class='fansright_time']/a[@class='fansright_zan']")?
                            .InnerText;
                        commentFloor.userHome = floor.SelectSingleNode("./h3/a")?.GetAttributeValue("href", string.Empty);
                        commentFloor.userName = floor.SelectSingleNode("./h3/a")?.InnerText.Trim();
                        commentFloor.content = floor.SelectSingleNode("./div[@class='gl_cont']")?.InnerText;
                        comment.commentFloors.Add(commentFloor);
                    }
                }
                comment.content = right.SelectSingleNode("./div[@class='ansright_cont']/a")?.InnerText;
                comment.thumbCount = right.SelectSingleNode("./div[@class='ansright_time']/a[@class='ansright_zan']")?
                    .InnerText;
            }
            return comment;
        }
    }
}
