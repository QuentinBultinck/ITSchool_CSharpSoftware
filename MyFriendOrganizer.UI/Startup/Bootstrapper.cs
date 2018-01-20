using Autofac;
using MyFriendOrganizer.DataAccess;
using MyFriendOrganizer.UI.Data;
using MyFriendOrganizer.UI.ViewModel;

namespace MyFriendOrganizer.UI.Startup
{
    public class Bootstrapper
    {
        //Dependency Injection
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FriendOrganizerDBContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<FriendDataService>().As<IFriendDataService>();

            return builder.Build();
        }
    }
}
