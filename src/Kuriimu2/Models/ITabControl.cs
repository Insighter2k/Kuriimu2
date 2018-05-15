using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Kuriimu2.Models
{
    public interface ITabControl
    {
        BindableCollection<ITabItem> TabCollection { get; set; }
        ITabItem SelectedTabItem { get; set; }

    }
}
