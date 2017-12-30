using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Bases.ViewModels;
using The_Paper.Data;
using The_Paper.Models;
using The_Paper.Services;
using Windows.UI.Xaml;

namespace The_Paper.ViewModels
{
    public class NewsPageVM : NotificationObject
    {
        private NewsPageService newsPageService;
        private News topNews;
        private int curIndex;

        private Visibility topNewsVisibility;

        public Visibility TopNewsVisibility
        {
            get { return topNewsVisibility; }
            set
            {
                if (topNewsVisibility != value)
                {
                    topNewsVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public News TopNews
        {
            get { //Debug.WriteLine("Get"); 
                return topNews;  }
            set
            {
                if (topNews != value)
                {
                    topNews = value;
                    NotifyPropertyChanged();
                    //Debug.WriteLine("Notify");
                }
            }
        }

        private ObservableCollection<News> newsList;

        public ObservableCollection<News> NewsList
        {
            get { return newsList; }
            set
            {
                if (newsList != value)
                {
                    newsList = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> tabNameList;

        public ObservableCollection<string> TabNameList
        {
            get { return tabNameList; }
            set
            {
                if (tabNameList != value)
                {
                    tabNameList = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public NewsPageVM()
        {
            tabNameList = new ObservableCollection<string>();
            newsPageService = new NewsPageService();
        }
        public async void init()
        {
            foreach (var channel in ChannelsData.channelList[0].subChannel)
                TabNameList.Add(channel.name);
            NewsListModel newsListModel = await newsPageService.parseHtml(0, 0);
            TopNews = newsListModel.TopNews;
            NewsList = newsListModel.NewsList;
        }

        public async Task Load(int index, int subindex)
        {
            TopNewsVisibility = Visibility.Visible;
            curIndex = index;
            TabNameList.Clear();
            foreach (var channel in ChannelsData.channelList[index].subChannel)
                TabNameList.Add(channel.name);
            NewsListModel newsListModel = await newsPageService.parseHtml(index, subindex);
            TopNews = newsListModel.TopNews;
            NewsList = newsListModel.NewsList;
        }

        public async void switchTab(object sender, TabSwitchEventArgs args)
        {
            TopNewsVisibility = args.tabIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            NewsListModel newsListModel = await newsPageService.parseHtml(curIndex , args.tabIndex);
            TopNews = newsListModel.TopNews;
            NewsList = newsListModel.NewsList;
        }
    }
}
