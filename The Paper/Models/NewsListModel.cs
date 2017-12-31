using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class NewsListModel
    {
        private News topNews;

        public News TopNews
        {
            get { return topNews; }
            set { topNews = value; }
        }

        private ObservableCollection<News> newsList;

        public ObservableCollection<News> NewsList
        {
            get { return newsList; }
            set { newsList = value; }
        }

        private string lastTime;

        public string LastTime
        {
            get { return lastTime; }
            set { lastTime = value; }
        }

        private string topCids;

        public string TopCids
        {
            get { return topCids; }
            set { topCids = value; }
        }

        private string nodeids;

        public string NodeIds
        {
            get { return nodeids; }
            set { nodeids = value; }
        }

        private string updateUri;

        public string UpdateUri
        {
            get { return updateUri; }
            set { updateUri = value; }
        }

        private int pageIndex;

        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

    }
}
