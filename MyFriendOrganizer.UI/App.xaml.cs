using Autofac;
using MyFriendOrganizer.UI.Data;
using MyFriendOrganizer.UI.Startup;
using MyFriendOrganizer.UI.ViewModel;
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
    }
}
