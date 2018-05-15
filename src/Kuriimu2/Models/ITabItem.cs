using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kuriimu2.Models
{
    public interface ITabItem
    {
        ImageSource TabIcon { get; set; }
        string TabHeader { get; set; }
        bool IsSelected { get; set; }
        void TabClose();
    }
}
