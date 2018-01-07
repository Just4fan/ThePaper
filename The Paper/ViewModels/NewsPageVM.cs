using System.Collections.ObjectModel;
using System.Diagnostics;
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

        private bool isOpen;

        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                if (isOpen != value)
                {
                    isOpen = value;
                    NotifyPropertyChanged();
                }
            }
        }

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
            get { return topNews;  }
            set
            {
                if (topNews != value)
                {
                    topNews = value;
                    NotifyPropertyChanged();
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

        public void Load(int index, int subindex)
        {
            curIndex = index;
            TabNameList.Clear();
            foreach (var channel in ChannelsData.channelList[index].subChannel)
                TabNameList.Add(channel.name);
            LoadTab(subindex);
        }

        public async void LoadMore()
        {
            if (onLoad)
                return;
            onLoad = true;
            await newsPageService.LoadMore(newsListModel);
            onLoad = false;
        }

        public async void LoadTab(int index)
        {
            TopNewsVisibility = index == 0 ?
                Visibility.Visible : Visibility.Collapsed;
            newsListModel = await newsPageService.Load(curIndex, index);
            TopNews = newsListModel.TopNews;
            NewsList = newsListModel.NewsList;
        }
    }
}
