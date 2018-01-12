using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Paper.Bases.ViewModels;
using The_Paper.Models;

namespace The_Paper.ViewModels
{
    public class CommentPageVM : NotificationObject
    {
        private ObservableCollection<Comment> _comments;

        public ObservableCollection<Comment> Comments
        {
            get { return _comments; }
            set
            {
                if (_comments != value)
                {
                    _comments = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public CommentPageVM(ObservableCollection<Comment> Comments)
        {
            this.Comments = Comments;
        }


    }
}
