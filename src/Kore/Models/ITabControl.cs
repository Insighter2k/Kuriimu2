using Caliburn.Micro;

namespace Kore.Models
{
    public interface ITabControl
    {
        BindableCollection<ITabItem> TabCollection { get; set; }
        ITabItem SelectedTabItem { get; set; }

    }
}
