using Autofac;
using MyFriendOrganizer.DataAccess;
using MyFriendOrganizer.UI.Data.Lookups;
using MyFriendOrganizer.UI.Data.Repositories;
using MyFriendOrganizer.UI.View.Services;
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

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();

            // Keyed zodat we erin MainWindow aan kunnen in IIndex
            builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>().Keyed<IDetailViewModel>(nameof(FriendDetailViewModel)); 
            builder.RegisterType<MeetingDetailViewModel>().As<IMeetingDetailViewModel>().Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces(); // Omdat LookupDataService van meerdere interfaces erft
            builder.RegisterType<FriendRepository>().As<IFriendRepository>();
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();

            return builder.Build();
        }
    }
}
