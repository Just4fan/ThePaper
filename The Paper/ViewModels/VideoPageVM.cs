using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Bases.ViewModels;
using The_Paper.Models;
using The_Paper.Services;

namespace The_Paper.ViewModels
{
    public class VideoPageVM : NotificationObject
    {
        private VideoPageService videoPageService { get; set; }
        private VideoDetailService videoDetailService;
        private VideoList videoListModel;

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


        public VideoPageVM()
        {
            videoPageService = new VideoPageService();
            videoDetailService = new VideoDetailService();
            Load();
        }

        public async void Load()
        {
            videoListModel = await videoPageService.Load(1, 0);
            VideoList = videoListModel.videoList;
            TopVideo = videoListModel.topVideo;
            //await new NewsDetailService().Load(@"http://www.thepaper.cn/newsDetail_forward_1931546");
        }

        public async void PlayTop()
        {
            VideoDetail = await videoDetailService.Load(TopVideo.uri);
        }

        public async void Play(Video video)
        {
            VideoDetail = await videoDetailService.Load(video.uri);
        }
    }
}
