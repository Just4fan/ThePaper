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

    }
}
