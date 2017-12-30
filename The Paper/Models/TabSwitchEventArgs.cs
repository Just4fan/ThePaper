using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Paper.Models
{
    public class TabSwitchEventArgs : EventArgs
    {
        public int tabIndex { get; set; }
        public TabSwitchEventArgs(int tabIndex)
        {
            this.tabIndex = tabIndex;
        }
    }
}
