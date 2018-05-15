using System.Windows.Media;

namespace Kore.Models
{
    public interface ITabItem
    {
        ImageSource TabIcon { get; set; }
        string TabHeader { get; set; }
        bool IsSelected { get; set; }
        void TabClose();
    }
}
