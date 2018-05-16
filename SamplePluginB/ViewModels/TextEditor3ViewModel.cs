using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using Kontract.Interface;
using Kore;
using Kore.Models;

namespace SamplePluginB.ViewModels
{
    [Export(typeof(ITabItem))]
    [Export(typeof(Screen))]
    public sealed class TextEditor3ViewModel : Screen, ITabItem
    {
        [ImportingConstructor]
        protected TextEditor3ViewModel() { }

        [ImportingConstructor]
        protected TextEditor3ViewModel(string tabHeader, ImageSource icon, bool isSelected, KoreFile kFile)
        {
            TabHeader = tabHeader;
            TabIcon = icon;
            IsSelected = isSelected;

            KoreFile = kFile;

            DisplayName = KoreFile.FileInfo.Name + (KoreFile.HasChanges ? "*" : string.Empty);
            _adapter = KoreFile.Adapter as ITextAdapter;



            if (_adapter != null)
                Entries = new ObservableCollection<TextEntry>(_adapter.Entries);

            SelectedEntry = Entries.First();

        }

        public static TextEditor3ViewModel Create(string tabHeader, ImageSource icon, bool isSelected, KoreFile kFile) => new TextEditor3ViewModel(tabHeader, icon, isSelected, kFile);

        #region Properties

        private ITextAdapter _adapter;
        private TextEntry _selectedEntry;

        public KoreFile KoreFile;
        public ObservableCollection<TextEntry> Entries { get; }

        #endregion

        #region ITabItem

        public ImageSource TabIcon { get; set; }
        public string TabHeader { get; set; }
        public bool IsSelected { get; set; }
        public void TabClose()
        {
            this.TryClose();
        }

        #endregion

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

        public string EntryCount => Entries.Count + (Entries.Count > 2 ? " Entries" : " Entry");

        public void AddEntry()
        {
            //Entries.Add(new Entry($"Label {Entries.Count}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            //NotifyOfPropertyChange(nameof(EntryCount));
            NotifyOfPropertyChange(nameof(Entries));
        }

        public void Save(string filename = "")
        {
            try
            {
                if (filename == string.Empty)
                    ((ISaveFiles)KoreFile.Adapter).Save(KoreFile.FileInfo.FullName);
                else
                {
                    ((ISaveFiles)KoreFile.Adapter).Save(filename);
                    KoreFile.FileInfo = new FileInfo(filename);
                }
                KoreFile.HasChanges = false;
                NotifyOfPropertyChange(DisplayName);
            }
            catch (Exception)
            {
                // Handle on UI gracefully somehow~
            }
        }
    }
}
