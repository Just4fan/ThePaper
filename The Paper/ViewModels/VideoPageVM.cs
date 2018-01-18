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

namespace The_Paper.ViewModels
{
    public class VideoPageVM : NotificationObject
    {
        private VideoPageService videoPageService;
        private CommentService commentService;
        private VideoDetailService videoDetailService;
        private VideoList videoListModel;
        private Channel channel;

        private bool _isPlaying;

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _hasTopVideo;

        public bool HasTopVideo
        {
            get { return _hasTopVideo; }
            set
            {
                if (_hasTopVideo != value)
                {
                    _hasTopVideo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _isOpen;

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (_isOpen != value)
                {
                    _isOpen = value;
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

        private bool _onLoadMore;

        public bool OnLoadMore
        {
            get { return _onLoadMore; }
            set
            {
                if (_onLoadMore != value)
                {
                    _onLoadMore = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _loadMoreStatus;

        public string LoadMoreStatus
        {
            get { return _loadMoreStatus; }
            set
            {
                if (_loadMoreStatus != value)
                {
                    _loadMoreStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<string> _tabNameList;

        public ObservableCollection<string> TabNameList
        {
            get { return _tabNameList; }
            set
            {
                if (_tabNameList != value)
                {
                    _tabNameList = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private VideoDetail _videoDetail;

        public VideoDetail VideoDetail
        {
            get { return _videoDetail; }
            set
            {
                if (_videoDetail != value)
                {
                    _videoDetail = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<Video> videoList;

        public ObservableCollection<Video> VideoList
        {
            get { return videoList; }
            set
            {
                if (videoList != value)
                {
                    videoList = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Video topVideo;

        public Video TopVideo
        {
            get { return topVideo; }
            set
            {
                if (topVideo != value)
                {
                    topVideo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<Comment> _hotComments;

        public ObservableCollection<Comment> HotComments
        {
            get { return _hotComments; }
            set
            {
                if (_hotComments != value)
                {
                    _hotComments = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<Comment> _newComments;

        public ObservableCollection<Comment> NewComments
        {
            get { return _newComments; }
            set
            {
                if (_newComments != value)
                {
                    _newComments = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public VideoPageVM(Channel channel)
        {
            this.channel = channel;
            IsOpen = false;
            HasTopVideo = false;
            videoPageService = new VideoPageService();
            videoDetailService = new VideoDetailService();
            commentService = new CommentService();
            Load(0);
        }

        public async void Load(int columnIdx)
        {
            TabNameList = new ObservableCollection<string>();
            foreach (var column in channel.columns)
                TabNameList.Add(column.name);
            await LoadColumn(columnIdx);
            //await new NewsDetailService().Load(@"http://www.thepaper.cn/newsDetail_forward_1931546");
        }

        public async void LoadMore()
        {
            if (OnLoadMore)
                return;
            OnLoadMore = true;
            LoadMoreStatus = "正在加载";
            bool status = await videoPageService.LoadMore(videoListModel);
            if (status)
                LoadMoreStatus = string.Empty;
            else
                LoadMoreStatus = "没有更多了~";
            OnLoadMore = false;
        }

        public async Task LoadColumn(int colIndex)
        {
            Loaded = false;
            HasTopVideo = false;
            videoListModel?.videoList?.Clear();
            videoListModel = await videoPageService.Load(channel, colIndex);
            VideoList = videoListModel.videoList;
            TopVideo = videoListModel.topVideo;
            if (colIndex == 0)
                HasTopVideo = true;
            Loaded = true;
        }

        public async void PlayTop()
        {
            IsOpen = true;
            VideoDetail = await videoDetailService.Load(TopVideo.uri);
            await commentService.InitAsync(TopVideo.uri);
            HotComments = commentService.LoadHot();
            NewComments =   commentService.LoadNew();
        }

        public async void Play(Video video)
        {
            IsOpen = true;
            VideoDetail = await videoDetailService.Load(video.uri);
            await commentService.InitAsync(video.uri);
            HotComments = commentService.LoadHot();
            NewComments = commentService.LoadNew();
        }
    }
}
