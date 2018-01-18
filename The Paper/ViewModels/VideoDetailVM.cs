using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Bases.ViewModels;
using The_Paper.Models;
using The_Paper.Services;

namespace The_Paper.ViewModels
{
    public class VideoDetailVM : NotificationObject
    {
        private CommentService commentService;

        private bool _hasNewComments;

        public bool HasNewComments
        {
            get { return _hasNewComments; }
            set
            {
                if (_hasNewComments != value)
                {
                    _hasNewComments = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _hasHotComments;

        public bool HasHotComments
        {
            get { return _hasHotComments; }
            set
            {
                if (_hasHotComments != value)
                {
                    _hasHotComments = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _hasMoreComments;

        public bool HasMoreComments
        {
            get { return _hasMoreComments; }
            set
            {
                if (_hasMoreComments != value)
                {
                    _hasMoreComments = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _loadCommentStatus ;

        public string LoadCommentStatus
        {
            get { return _loadCommentStatus; }
            set
            {
                if (_loadCommentStatus != value)
                {
                    _loadCommentStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _onLoadMoreComments;

        public bool OnLoadMoreComments
        {
            get { return _onLoadMoreComments; }
            set
            {
                if (_onLoadMoreComments != value)
                {
                    _onLoadMoreComments = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Comments Comments;

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

        public VideoDetailVM()
        {
            commentService = new CommentService();
            TabNameList = new ObservableCollection<string>()
            {
                "直播",
                "评论"
            };
        }

        public async void Load(string uri)
        {
            Comments = await commentService.GetCommentsAsync(uri);
            HotComments = Comments.hotComments;
            NewComments = Comments.newComments;
            if (HotComments.Count > 0)
                HasHotComments = true;
            if (NewComments.Count > 0)
                HasNewComments = true;
        }

        public async void LoadMoerComment()
        {
            if (OnLoadMoreComments)
                return;
            OnLoadMoreComments = true;
            LoadCommentStatus = "正在加载...";
            bool status = await commentService.LoadMoreAsync(Comments);
            if (status)
                LoadCommentStatus = string.Empty;
            else
                LoadCommentStatus = "没有更多了~";
            OnLoadMoreComments = false;
        }
    }
}
