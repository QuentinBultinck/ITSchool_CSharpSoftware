using MyFriendOrganizer.Model;
using System.Collections.ObjectModel;
using MyFriendOrganizer.UI.Data;
using System.Threading.Tasks;

namespace MyFriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IFriendDataService _friendDataService;

        public MainViewModel(IFriendDataService friendDataService)
        {
            Friends = new ObservableCollection<Friend>();
            _friendDataService = friendDataService;
        }

        public async Task LoadAsync()
        {
            var friends = await _friendDataService.GetAllAsync();
            Friends.Clear();
            foreach (var friend in friends)
            {
                Friends.Add(friend);
            }
        }

        // Notifies if the collection changes => for Databinding
        public ObservableCollection<Friend> Friends { get; set; }

        private Friend _selectedFriend;

        public Friend SelectedFriend
        {
            get { return _selectedFriend; }
            set
            {
                _selectedFriend = value;
                OnPropertyChanged(); 
            }
        }
    }
}
