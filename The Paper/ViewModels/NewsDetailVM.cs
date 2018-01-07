using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Bases.ViewModels;
using The_Paper.Models;
using The_Paper.Services;

namespace The_Paper.ViewModels
{
    public class NewsDetailVM : NotificationObject
    {
        private NewsDetailService newsDetailService;

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

        public NewsDetailVM()
        {
            newsDetailService = new NewsDetailService();
        }

        public async void Load(string uri)
        {
            Loaded = false;
            NewsDetail = await newsDetailService.Load(uri);
            Loaded = true;
        }
    }
}
