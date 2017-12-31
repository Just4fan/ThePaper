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
        private bool onLoad;
        private NewsListModel newsListModel;

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
            newsListModel = await newsPageService.Load(0, 0);
            TopNews = newsListModel.TopNews;
            NewsList = newsListModel.NewsList;
        }

        public async Task Load(int index, int subindex)
        {
            if (onLoad)
                return;
            onLoad = true;
            TopNewsVisibility = Visibility.Visible;
            curIndex = index;
            TabNameList.Clear();
            foreach (var channel in ChannelsData.channelList[index].subChannel)
                TabNameList.Add(channel.name);
            newsListModel = await newsPageService.Load(index, subindex);
            TopNews = newsListModel.TopNews;
            NewsList = newsListModel.NewsList;
            onLoad = false;
        }

        public async Task LoadMore()
        {
            if (onLoad)
                return;
            onLoad = true;
            await newsPageService.LoadMore(newsListModel);
            onLoad = false;
        }

        public async void switchTab(object sender, TabSwitchEventArgs args)
        {
            if (onLoad)
                return;
            onLoad = true;
            TopNewsVisibility = args.tabIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            newsListModel = await newsPageService.Load(curIndex , args.tabIndex);
            TopNews = newsListModel.TopNews;
            NewsList = newsListModel.NewsList;
            onLoad = false;
        }
    }
}
