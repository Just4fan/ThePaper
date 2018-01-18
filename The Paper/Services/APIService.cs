using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Models;

namespace The_Paper.Services
{
    public class APIService
    {
        public static APIService apiService = new APIService();

        public static string GetVerifyCodeURL()
        {
            string url = "http://www.thepaper.cn/www/RandomPicture?";
            long ts = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
            return url + ts;
        }

        public static string GetLoginURL()
        {
            string url = "http://www.thepaper.cn/www/login.msp";
            return url;
        }

        public string GetURL(NewsListModel newsListModel)
        {
            return string.Format("{0}nodeids={1}&topCids={2}&pageIndex={3}&lastTime={4}"
                , newsListModel.UpdateUri
                , newsListModel.NodeIds
                , newsListModel.TopCids
                , newsListModel.PageIndex + 1
                , newsListModel.LastTime);
        }

        public string GetURL(VideoList videoList)
        {
            return string.Format("{0}channelID={1}&pageidx={2}",
                videoList.updateUri,
                videoList.channelID,
                ++videoList.pageIndex);
        }

        public string GetCommentURL(string url, Comments comments)
        {
            string commentURL = "http://www.thepaper.cn/newDetail_commt.jsp?";
            int index = url.LastIndexOf('_');
            if (index != -1)
            {
                comments.contid = url.Substring(index + 1);
                return string.Format("{0}contid={1}&_={2}",
                    commentURL,
                    comments.contid,
                    (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds));
            }
            else
                return string.Empty;
        }

        public string LoadMoreCommentURL(Comments comments)
        {
            string url = "http://www.thepaper.cn/load_moreFloorComment.jsp?";
            return string.Format("{0}contid={1}&hotIds={2}&pageidx={3}&startId={4}",
                url,
                comments.contid,
                comments.hotIds,
                comments.pageidx + 1,
                comments.startId);
        }
    }
}