using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Waf;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Threading;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Presentation.Properties;
using WafTraffic.Presentation.Views;
using DotNet.Business;

namespace WafTraffic.Presentation
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private AggregateCatalog catalog;
        private CompositionContainer container;
       private IEnumerable<IModuleController> moduleControllers;
        // private IModuleController controller;
        private CurrentLoginService currentLogin;

        static App()
        {
#if (DEBUG)
            WafConfiguration.Debug = true;
#endif
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if (DEBUG != true)
            // Don't handle the exceptions in Debug mode because otherwise the Debugger wouldn't
            // jump into the code when an exception occurs.
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif
            //add by ydf  for DOTNET.Utilities frame
            currentLogin = new CurrentLoginService();

            catalog = new AggregateCatalog();
            // Add the WpfApplicationFramework assembly to the catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Controller).Assembly));
            // Add the WafTraffic.Presentation assembly to the catalog
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            // Add the WafTraffic.Applications assembly to the catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ShellViewModel).Assembly));

            // Load module assemblies as well (e.g. Reporting extension). See App.config file.
            foreach (string moduleAssembly in Settings.Default.ModuleAssemblies)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(moduleAssembly));
            }

            container = new CompositionContainer(catalog);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);

            moduleControllers = container.GetExportedValues<IModuleController>();
            foreach (IModuleController moduleController in moduleControllers) { moduleController.Initialize(); }
            foreach (IModuleController moduleController in moduleControllers) { moduleController.Run(); }

            //catalog = new AggregateCatalog();
            //// Add the WpfApplicationFramework assembly to the catalog
            //catalog.Catalogs.Add(new AssemblyCatalog(typeof(Controller).Assembly));
            //// Add the Writer.Presentation assembly to the catalog
            //catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            //// Add the Writer.Applications assembly to the catalog
            //catalog.Catalogs.Add(new AssemblyCatalog(typeof(IModuleController).Assembly));

            //container = new CompositionContainer(catalog);
            //CompositionBatch batch = new CompositionBatch();
            //batch.AddExportedValue(container);
            //container.Compose(batch);

            //controller = container.GetExportedValue<IModuleController>();
            //controller.Initialize();
            //controller.Run();

           
        }

        protected override void OnExit(ExitEventArgs e)
        {
            foreach (IModuleController moduleController in moduleControllers.Reverse()) { moduleController.Shutdown(); }
            container.Dispose();
            catalog.Dispose();

            base.OnExit(e);

            //controller.Shutdown();
            //container.Dispose();
            //catalog.Dispose();

            //base.OnExit(e);

            Environment.Exit(0);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, false);
            e.Handled = true;
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception, e.IsTerminating);
        }

        private static void HandleException(Exception e, bool isTerminating)
        {
            if (e == null) { return; }

            //Trace.TraceError(e.ToString());

            //if (!isTerminating)
            //{
            //    MessageBox.Show(string.Format(CultureInfo.CurrentCulture,
            //            WafTraffic.Presentation.Properties.Resources.UnknownError, e.ToString())
            //        , ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
            //}
           
            //errlog处理
            CurrentLoginService.Instance.LogException(e);

            if (!isTerminating)
            {
                //MessageBox.Show("系统出错，请确保数据及操作正确，或重启系统！", ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
