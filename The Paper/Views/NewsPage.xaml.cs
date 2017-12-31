using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using The_Paper.Data;
using The_Paper.Http;
using The_Paper.Models;
using The_Paper.Services;
using The_Paper.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace The_Paper.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewsPage : Page
    {
        NewsPageVM newsPageVM;
        double verticaloffset;

        public NewsPage()
        {
            this.InitializeComponent();
            newsPageVM = new NewsPageVM();
            DataContext = newsPageVM;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            newsPageVM.Load((int)e.Parameter, 0);
        }

        private void TabView_TabSwitch(object sender, EventArgs e)
        {
            scrollViewer.ChangeView(null, 0, null);
            newsPageVM.switchTab(sender, e as TabSwitchEventArgs);
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if ((sender as ScrollViewer).VerticalOffset + (sender as ScrollViewer).ViewportHeight
                == (sender as ScrollViewer).ExtentHeight)
                newsPageVM.LoadMore();
        }
    }
}
