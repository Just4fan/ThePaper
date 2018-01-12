using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using The_Paper.Models;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace The_Paper.Controls
{
    public sealed class CommentControl : Control
    {

        private Grid Grid;

        public Comment Comment
        {
            get { return (Comment)GetValue(CommentProperty); }
            set { SetValue(CommentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Comment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommentProperty =
            DependencyProperty.Register("Comment", typeof(Comment), typeof(CommentControl), new PropertyMetadata(new Comment()));



        public int MaxContentLine
        {
            get { return (int)GetValue(MaxContentLineProperty); }
            set { SetValue(MaxContentLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxContentLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxContentLineProperty =
            DependencyProperty.Register("MaxContentLine", typeof(int), typeof(CommentControl), new PropertyMetadata(2));



        public Brush FloorBackground
        {
            get { return (Brush)GetValue(FloorBackgroundProperty); }
            set { SetValue(FloorBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FloorColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FloorBackgroundProperty =
            DependencyProperty.Register("FloorBackground", typeof(Brush), typeof(CommentControl), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));



        public CommentControl()
        {
            this.DefaultStyleKey = typeof(CommentControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Grid = GetTemplateChild("Grid") as Grid;
        }
    }
}
