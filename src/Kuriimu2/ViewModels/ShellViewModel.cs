using System;
using System.ComponentModel.Composition;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Kontract.Interface;
using Kore.Models;
using Kore.ViewModels;
using Microsoft.Win32;

namespace Kuriimu2.ViewModels
{
    [Export]
    public sealed class ShellViewModel : Conductor<IScreen>.Collection.OneActive, ITabControl
    {
        #region Private

        private Kore.Kore _kore;

        #endregion

        public ShellViewModel()
        {
            DisplayName = "Kuriimu";
            _kore = new Kore.Kore();
        }

        #region ITabControl
        public BindableCollection<ITabItem> TabCollection { get; set; } = new BindableCollection<ITabItem>();

        public ITabItem SelectedTabItem { get; set; }

        public void CloseTab()
        {
            SelectedTabItem.TabClose();
            TabCollection.Remove(SelectedTabItem);
        }

        public bool CanCloseTab() => SelectedTabItem != null;

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void View_Loaded()
        {
           TabCollection.Add(TextEditor3ViewModel.Create("Tester3", null, true, null));
        }

        #endregion

        public void OpenButton()
        {
            var ofd = new OpenFileDialog { Filter = _kore.FileFilters };

            if (ofd.ShowDialog() == true)
            {
                var kf = _kore.LoadFile(ofd.FileName);

                switch (kf.Adapter)
                {
                    case ITextAdapter txt2:
                        TabCollection.Add(TextEditor2ViewModel.Create("Test#1 *", new BitmapImage(new Uri("pack://application:,,,/Images/menu-power.png")), true, kf));
                        break;
                }
            }
        }

        public void SaveButton()
        {
            switch (ActiveItem)
            {
                case TextEditor2ViewModel txt2:
                    txt2.Save();
                    break;
            }
        }

        public void SaveAsButton()
        {
            var filter = "Any File (*.*)|*.*";

            switch (ActiveItem)
            {
                case TextEditor2ViewModel txt2:
                    filter = txt2.KoreFile.Filter;
                    break;
            }

            var sfd = new SaveFileDialog { Filter = filter };

            if (sfd.ShowDialog() == true)
            {
                switch (ActiveItem)
                {
                    case TextEditor2ViewModel txt2:
                        txt2.Save(sfd.FileName);
                        break;
                }
            }
        }

        public void DebugButton()
        {
            _kore.Debug();
        }
    }
}
