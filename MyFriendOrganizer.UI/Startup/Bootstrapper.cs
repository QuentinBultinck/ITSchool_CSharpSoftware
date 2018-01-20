using Autofac;
using MyFriendOrganizer.DataAccess;
using MyFriendOrganizer.UI.Data;
using MyFriendOrganizer.UI.ViewModel;
using Prism.Events;

namespace MyFriendOrganizer.UI.Startup
{
    public class Bootstrapper
    {
        //Dependency Injection
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            //Event supervisor
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            //Database connectie
            builder.RegisterType<FriendOrganizerDBContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<FriendDataService>().As<IFriendDataService>();

            return builder.Build();
        }
    }
}
