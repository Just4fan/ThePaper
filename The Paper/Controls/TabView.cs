using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using The_Paper.Models;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace The_Paper.Controls
{
    public sealed class TabView : Control
    {
        private Grid grid;
        private StackPanel stackPanel;
        public int previousIndex { get; set; }
        public int currentIndex { get; set; }
        private event PointerEventHandler onTabSwitchEvent;
        public event EventHandler TabSwitch;

        public ObservableCollection<string> TabNameList
        {
            get { return (ObservableCollection<string>)GetValue(TabNameListProperty); }
            set
            {
                SetValue(TabNameListProperty, value);
                TabNameList.CollectionChanged += TabNameList_CollectionChanged;
                updateTab();
            }
        }



        public UIElement View
        {
            get { return (UIElement)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TabNameList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TabNameListProperty =
            DependencyProperty.Register("TabNameList",
                typeof(ObservableCollection<string>),
                typeof(TabView),
                new PropertyMetadata(0));

        // Using a DependencyProperty as the backing store for Frame.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register("View",
                typeof(UIElement),
                typeof(TabView),
                new PropertyMetadata(new Grid()));



        public TabView()
        {
            this.DefaultStyleKey = typeof(TabView);
            currentIndex = 0;
            onTabSwitchEvent += onTabSwitch;
        }

        private void TabNameList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            updateTab();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            grid = GetTemplateChild("grid") as Grid;
            stackPanel = GetTemplateChild("stackPanel") as StackPanel;
            stackPanel.SetValue(Grid.RowProperty, 0);
            stackPanel.Orientation = Orientation.Horizontal;
            View.SetValue(Grid.RowProperty, 1);
            grid.Background = new SolidColorBrush(Color.FromArgb(255, 245, 245, 245)); 
            stackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 245, 245, 245));
            grid.Children.Add(View);
            updateTab();
        }

        private void updateTab()
        {
            stackPanel.Children.Clear();
            if (TabNameList == null || TabNameList.Count == 0)
                return;
            foreach (string name in TabNameList)
                stackPanel.Children.Add(generateTabItem(name));
            (stackPanel.Children[0] as TextBlock).FontWeight = Windows.UI.Text.FontWeights.Bold;
            (stackPanel.Children[0] as TextBlock).Foreground = new SolidColorBrush(Colors.Black);
        }

        private TextBlock generateTabItem(string name)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = name;
            textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150));
            textBlock.Margin = new Thickness(10, 5, 10, 0);
            textBlock.AddHandler(PointerPressedEvent, onTabSwitchEvent, false);
            //Debug.WriteLine("Created");
            return textBlock;
        }

        private void onTabSwitch(object sender, PointerRoutedEventArgs e)
        {
            previousIndex = currentIndex;
            currentIndex = stackPanel.Children.IndexOf(sender as TextBlock);
            (stackPanel.Children[previousIndex] as TextBlock).FontWeight = Windows.UI.Text.FontWeights.Normal;
            (stackPanel.Children[previousIndex] as TextBlock).Foreground = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150));
            (stackPanel.Children[currentIndex] as TextBlock).FontWeight = Windows.UI.Text.FontWeights.Bold;
            (stackPanel.Children[currentIndex] as TextBlock).Foreground = new SolidColorBrush(Colors.Black);
            TabSwitch?.Invoke(this, new TabSwitchEventArgs(currentIndex));
        }
    }
}
