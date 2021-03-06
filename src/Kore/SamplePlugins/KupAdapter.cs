﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Kontract.Attribute;
using Kontract.Interface;

namespace Kore.SamplePlugins
{
    [Export(typeof(ITextAdapter))]
    [Export(typeof(IIdentifyFiles))]
    [Export(typeof(ILoadFiles))]
    [Export(typeof(ISaveFiles))]
    [PluginInfo("963E7A09-61C4-4A80-94BA-427134F1A5B8", "Kuriimu Text Archive", "KUP", "IcySon55", "This is the KUP text adapter for Kuriimu.")]
    [PluginExtensionInfo("*.kup")]
    public sealed class KupAdapter : ITextAdapter, IIdentifyFiles, ILoadFiles, ISaveFiles
    {
        private KUP _kup;

        #region Properties

        public IEnumerable<TextEntry> Entries => _kup?.Entries;

        public string NameFilter => @".*";
        public int NameMaxLength => 0;

        public string LineEndings
        {
            get => "\n";
            set => throw new NotImplementedException();
        }

        #endregion

        public bool Identify(string filename)
        {
            var result = true;

            try
            {
                using (var sr = new StreamReader(File.OpenRead(filename)))
                {
                    sr.ReadLine(); // Skip the XML declaration
                    if (!sr.ReadLine().StartsWith("<kup"))
                        result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public void Load(string filename)
        {
            if (File.Exists(filename))
                _kup = KUP.Load(filename);
        }

        public void Save(string filename)
        {
            _kup.Save(filename);
        }
    }
}
