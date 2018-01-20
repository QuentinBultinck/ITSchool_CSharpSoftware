using Autofac;
using MyFriendOrganizer.UI.Data;
using MyFriendOrganizer.UI.Startup;
using MyFriendOrganizer.UI.ViewModel;
using System;
using System.Windows;

namespace MyFriendOrganizer.UI
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();

            //Autofax regelt de dependencies via de Bootstrapper
            var mainWindow = container.Resolve<MainWindow>();
            MainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occurred. Please inform the admin."
                + Environment.NewLine + e.Exception.Message, "Unexpected error");
            e.Handled = true; // Wij behandelen de error met een custom error dus true
        }
    }
}
