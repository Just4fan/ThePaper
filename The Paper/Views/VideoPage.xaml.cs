using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using The_Paper.Models;
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
    public sealed partial class VideoPage : Page
    {
        VideoPageVM videoPageVM;
        public VideoPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            videoPageVM = new VideoPageVM(e.Parameter as Channel);
            this.DataContext = videoPageVM;
        }

        private void playButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            videoPageVM.PlayTop();
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as GridView).SelectedItem != null)
            {
                videoPageVM.Play((Video)(sender as GridView).SelectedItem);
                VideoDetail.Navigate(typeof(VideoDetailPage), ((sender as GridView).SelectedItem as Video).uri);
            }
        }

        private async void TabView_TabSwitch(object sender, EventArgs e)
        {
            await videoPageVM.LoadColumn((e as TabSwitchEventArgs).tabIndex);
        }

        private void mediaElement_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            EnterAnim.Begin();
            EnterAnim.Completed += (sender1, e1) => VideoInfo.Visibility = Visibility.Visible;
        }

        private void mediaElement_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ExitAnim.Begin();
            EnterAnim.Completed += (sender1, e1) => VideoInfo.Visibility = Visibility.Collapsed;
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if ((sender as ScrollViewer).VerticalOffset + (sender as ScrollViewer).ViewportHeight
                == (sender as ScrollViewer).ExtentHeight)
                videoPageVM.LoadMore();
        }
    }
}
