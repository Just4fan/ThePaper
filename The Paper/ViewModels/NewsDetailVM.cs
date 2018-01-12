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
    public class NewsDetailVM : NotificationObject
    {
        private NewsDetailService newsDetailService;
        private CommentService commentService;

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

        private NewsDetail newsDetail;

        public NewsDetail NewsDetail
        {
            get { return newsDetail; }
            set
            {
                if (newsDetail != value)
                {
                    newsDetail = value;
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

        public NewsDetailVM()
        {
            newsDetailService = new NewsDetailService();
            commentService = new CommentService();
        }

        public async void Load(string uri)
        {
            Loaded = false;
            NewsDetail = await newsDetailService.Load(uri);
            await commentService.InitAsync(string.Format("{0}contid={1}&_={2}",
                ChannelsData.commentURL, NewsDetail.id,
                (long)((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds)));
            HotComments = commentService.LoadHot();
            NewComments = commentService.LoadNew();
            Loaded = true;
        }
    }
}
