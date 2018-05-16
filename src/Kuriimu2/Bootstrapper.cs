using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Kuriimu2.Models;
using Kuriimu2.ViewModels;

namespace Kuriimu2
{
    // To be configured with MEF for extensibility
    public class AppBootstrapper : BootstrapperBase
    {
        private CompositionContainer container;

        public AppBootstrapper() => Initialize();

        protected override void Configure()
        {
            var catalog = 
                new AggregateCatalog(
                    AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()
                );
            
            container = new CompositionContainer(catalog);

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);

            container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            string path = Path.GetDirectoryName(
                        Assembly.GetExecutingAssembly().Location);


            //Get all plugins, you need a naming convention!!
            string[] files = Directory.GetFiles(Environment.CurrentDirectory);
            string[] pluginFilesSorted = files.Where(x => x.Contains(".dll") && x.Split('\\').Last().StartsWith("Sample")).ToArray();

            Assembly[] assemblies = new Assembly[pluginFilesSorted.Length + 1];

            for (int i = 0; i < pluginFilesSorted.Length; i++)
            {
                assemblies[i] = Assembly.LoadFile(pluginFilesSorted[i]);
            }

            assemblies[pluginFilesSorted.Length] = Assembly.GetExecutingAssembly();

            return assemblies;
        }

    }
}
