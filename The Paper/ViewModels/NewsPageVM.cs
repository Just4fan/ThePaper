using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        private string _loadStatus;

        public string LoadStatus
        {
            get { return _loadStatus; }
            set
            {
                if (_loadStatus != value)
                {
                    _loadStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _loaded;

        public bool Loaded
        {
            get { return _loaded; }
            set
            {
                if (_loaded != value)
                {
                    _loaded = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _hasTopNews;

        public bool HasTopNews
        {
            get { return _hasTopNews; }
            set
            {
                if (_hasTopNews != value)
                {
                    _hasTopNews = value;
                    NotifyPropertyChanged();
                }
            }
        }

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

        public async void Load(int index, int subindex)
        {
            curIndex = index;
            TabNameList.Clear();
            foreach (var channel in ChannelsData.channelList[index].subChannel)
                TabNameList.Add(channel.name);
            await LoadTab(subindex);
        }

        public async void LoadMore()
        {
            if (onLoad)
                return;
            onLoad = true;
            LoadStatus = "正在加载...";
            if (await newsPageService.LoadMore(newsListModel))
                LoadStatus = string.Empty;
            else
                LoadStatus = "没有更多数据了~";
            onLoad = false;
        }

        public async Task LoadTab(int index)
        {
            onLoad = true;
            TopNews = null;
            NewsList?.Clear();
            HasTopNews = false;
            Loaded = false;
            newsListModel = await newsPageService.Load(curIndex, index);
            TopNews = newsListModel.TopNews;
            NewsList = newsListModel.NewsList;
            HasTopNews = index == 0 ?
                true : false;
            Loaded = true;
            GC.Collect();
            onLoad = false;
        }
    }
}
