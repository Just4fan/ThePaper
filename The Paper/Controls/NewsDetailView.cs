using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using The_Paper.Models;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace The_Paper.Controls
{
    public sealed class NewsDetailView : Control
    {
        private Grid Grid;

        public NewsDetail NewsDetail
        {
            get { return (NewsDetail)GetValue(NewsDetailProperty); }
            set
            {
                SetValue(NewsDetailProperty, value);
                Debug.WriteLine("Changed");
                LoadNewsDetail();
            }
        }

        // Using a DependencyProperty as the backing store for newsDetail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewsDetailProperty =
            DependencyProperty.Register("NewsDetail",
                typeof(NewsDetail),
                typeof(NewsDetailView),
                new PropertyMetadata(new NewsDetail(),
                    new PropertyChangedCallback(NewsDetailChanged)));


        public NewsDetailView()
        {
            this.DefaultStyleKey = typeof(NewsDetailView);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Grid = GetTemplateChild("Grid") as Grid;
            Grid.Background = new SolidColorBrush(Color.FromArgb(255, 245, 245, 245));
        }

        private static void NewsDetailChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            NewsDetailView view = obj as NewsDetailView;
            view.LoadNewsDetail();
        }

        private void LoadNewsDetail()
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Vertical;
            RichTextBlock richTextBlock = new RichTextBlock();
            richTextBlock.CharacterSpacing = 100;
            if (NewsDetail == null)
                return;
            foreach(NewsContent content in NewsDetail.content)
            {
                if(content.type == NewsContent.type_text)
                {
                    NewsText newsText = content.content as NewsText;
                    Run run = new Run();
                    Paragraph paragraph = new Paragraph();
                    paragraph.Margin = new Thickness(0, 15, 0, 15);
                    if (newsText.isBold)
                        run.FontWeight = FontWeights.Bold;
                    run.Text = newsText.content;
                    paragraph.Inlines.Add(run);
                    richTextBlock.Blocks.Add(paragraph);
                }

                else if(content.type == NewsContent.type_video)
                {
                    NewsVideo newsVideo = content.content as NewsVideo;
                    MediaElement mediaElement = new MediaElement();
                    MediaTransportControls mtc = new MediaTransportControls();
                    InlineUIContainer container = new InlineUIContainer();
                    StackPanel stackPanel = new StackPanel();
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = newsVideo.desc;
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.Foreground = new SolidColorBrush(Colors.Gray);
                    textBlock.Margin = new Thickness(0, 10, 0, 10);
                    textBlock.FontSize = 12;
                    mediaElement.AreTransportControlsEnabled = true;
                    mediaElement.TransportControls = mtc;
                    mediaElement.AutoPlay = false;
                    mediaElement.Source = new Uri(newsVideo.videoSrc);
                    stackPanel.Orientation = Orientation.Vertical;
                    stackPanel.Children.Add(mediaElement);
                    stackPanel.Children.Add(textBlock);
                    stackPanel.Margin = new Thickness(20, 15, 20, 15);
                    if (richTextBlock.Blocks.Count > 0)
                    {
                        panel.Children.Add(richTextBlock);
                        richTextBlock = new RichTextBlock();
                        richTextBlock.CharacterSpacing = 100;
                    }
                    panel.Children.Add(stackPanel);
                }

                else if(content.type == NewsContent.type_image)
                {
                    NewsImage newsImage = content.content as NewsImage;
                    Image image = new Image();
                    TextBlock textBlock = new TextBlock();
                    StackPanel stackPanel = new StackPanel();
                    BitmapImage bitMap = new BitmapImage();
                    Paragraph paragraph = new Paragraph();
                    InlineUIContainer container = new InlineUIContainer();
                    bitMap.UriSource = new Uri(newsImage.imageSrc);
                    image.Source = bitMap;
                    textBlock.Text = newsImage.desc;
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.Foreground = new SolidColorBrush(Colors.Gray);
                    textBlock.FontSize = 12;
                    textBlock.Margin = new Thickness(0, 10, 0, 10);
                    stackPanel.Orientation = Orientation.Vertical;
                    stackPanel.Margin = new Thickness(20, 15, 20, 15);
                    stackPanel.Children.Add(image);
                    stackPanel.Children.Add(textBlock);
                    container.Child = stackPanel;
                    paragraph.Inlines.Add(container);
                    richTextBlock.Blocks.Add(paragraph);
                }
            }
            if (richTextBlock.Blocks.Count > 0)
                panel.Children.Add(richTextBlock);
            Grid.Children.Clear();
            Grid.Children.Add(panel);
        }
    }
}
