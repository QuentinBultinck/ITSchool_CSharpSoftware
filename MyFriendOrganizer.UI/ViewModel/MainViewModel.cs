using MyFriendOrganizer.Model;
using System.Collections.ObjectModel;
using MyFriendOrganizer.UI.Data;
using System.Threading.Tasks;

namespace MyFriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationViewModel navigationViewModel, IFriendDetailViewModel friendDetailViewModel)
        {
            NavigationViewModel = navigationViewModel;
            FriendDetailViewModel = friendDetailViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public INavigationViewModel NavigationViewModel { get; set; }

        public IFriendDetailViewModel FriendDetailViewModel { get; set; }
    }
}
