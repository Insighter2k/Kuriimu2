using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Kuriimu2.ViewModels;

namespace Kuriimu2
{
    // To be configured with MEF for extensibility
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper() => Initialize();
        protected override void OnStartup(object sender, StartupEventArgs e) => DisplayRootViewFor<ShellViewModel>();

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            string path = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);
            return base.SelectAssemblies().Concat(
                new Assembly[]
                {
                    Assembly.LoadFile(Path.Combine(path, "Kore.dll"))
                });
        }
    }
}
