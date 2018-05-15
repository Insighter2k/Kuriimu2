using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using Kontract.Interface;
using Kore;
using Kore.Models;

namespace Kuriimu2.ViewModels
{
    public sealed class TextEditor1ViewModel : Screen, ITabItem
    {
        protected TextEditor1ViewModel() { }

        protected TextEditor1ViewModel(string tabHeader, ImageSource icon, bool isSelected, KoreFile kFile)
        {
            TabHeader = tabHeader;
            TabIcon = icon;
            IsSelected = IsSelected;

            DisplayName = "Text Editor 1.x";

            _koreFile = kFile;
            _adapter = _koreFile.Adapter as ITextAdapter;

            foreach (var entry in _adapter.Entries)
            {
                Entries.Add(entry);
            }

            SelectedEntry = Entries.First();
        }

        public static TextEditor1ViewModel Create(string tabHeader, ImageSource icon, bool isSelected, KoreFile kFile)=> new TextEditor1ViewModel(tabHeader, icon,isSelected,kFile);

        #region ITabItem

        public ImageSource TabIcon { get; set; }
        public string TabHeader { get; set; }
        public bool IsSelected { get; set; }
        public void TabClose()
        {
            this.TryClose();
        }

        #endregion


        #region Properties

        private KoreFile _koreFile;
        private ITextAdapter _adapter;
        private TextEntry _selectedEntry;
        public ObservableCollection<TextEntry> Entries { get; } = new ObservableCollection<TextEntry>();

        public TextEntry SelectedEntry
        {
            get => _selectedEntry;
            set
            {
                _selectedEntry = value;
                EditedText = _selectedEntry.EditedText;
                NotifyOfPropertyChange(() => SelectedEntry);
            }
        }

        public string EditedText
        {
            get => _selectedEntry?.EditedText;
            set
            {
                if (_selectedEntry == null) return;
                _selectedEntry.EditedText = value;
                NotifyOfPropertyChange(() => EditedText);
            }
        }

        public string EntryCount => Entries.Count + (Entries.Count > 1 ? " Entries" : " Entry");
        #endregion


        #region Methods

        public void AddEntry()
        {
            //Entries.Add(new Entry($"Label {Entries.Count}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            NotifyOfPropertyChange(nameof(EntryCount));
        }
        #endregion
    }
}
