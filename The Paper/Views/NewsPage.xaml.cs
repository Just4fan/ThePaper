using System;
using System.Diagnostics;
using The_Paper.Models;
using The_Paper.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            newsPageVM.LoadTab((e as TabSwitchEventArgs).tabIndex);
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if ((sender as ScrollViewer).VerticalOffset + (sender as ScrollViewer).ViewportHeight
                == (sender as ScrollViewer).ExtentHeight)
                newsPageVM.LoadMore();
        }

        private void NewsCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Grid.ColumnDefinitions.Clear();
            Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
            NewsDetail.SetValue(Grid.ColumnProperty, 1);
            newsPageVM.IsOpen = true;
            NewsDetail.Navigate(typeof(NewsDetailPage), 
                ((News)((sender as GridView).SelectedItem)).uri);
            NewsDetail.Visibility = Visibility.Visible;
        }

        private void topNews_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Grid.ColumnDefinitions.Clear();
            Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
            NewsDetail.SetValue(Grid.ColumnProperty, 1);
            newsPageVM.IsOpen = true;
            NewsDetail.Navigate(typeof(NewsDetailPage),
                (newsPageVM.TopNews.uri));
            NewsDetail.Visibility = Visibility.Visible;
        }
    }
}
