using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Bases.ViewModels;
using The_Paper.Data;
using The_Paper.Models;
using The_Paper.Views;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace The_Paper.ViewModels
{
    public class MainPageVM : NotificationObject
    {
        private Frame frame;
        private CoreDispatcher dispatcher;
        private ObservableCollection<Channel> channelList;

        public ObservableCollection<Channel> ChannelList
        {
            get { return channelList; }
            set
            {
                if (channelList != value)
                {
                    channelList = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public MainPageVM(Frame frame, CoreDispatcher dispatcher)
        {
            this.frame = frame;
            this.dispatcher = dispatcher;
            Load();
            //frame.Navigate(typeof(VideoPage));
        }

        public async void Load()
        {
            await ChannelsData.generateList();
            ChannelList = new ObservableCollection<Channel>(ChannelsData.channelList);
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => frame.Navigate(typeof(VideoPage), 0));
        }

        public void Navigate(int index)
        {
            frame.Navigate(typeof(NewsPage), index);
        }
    }
}
