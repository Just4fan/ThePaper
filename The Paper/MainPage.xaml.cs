using The_Paper.ViewModels;
using The_Paper.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace The_Paper
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageVM mainPageVM;
        public MainPage()
        {
            this.InitializeComponent();
            mainPageVM = new MainPageVM(mainFrame, Dispatcher);
            this.DataContext = mainPageVM;
        }

        private void Ham_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ListView).SelectedIndex;
            switch(index)
            {
                case 0: case 2: case 3: case 4: case 6:
                    mainPageVM.Navigate(index);
                    break;
                case 1:
                    mainFrame.Navigate(typeof(VideoPage));
                    break;
            }
            
        }

        private void Ham_Click(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }
    }
}
