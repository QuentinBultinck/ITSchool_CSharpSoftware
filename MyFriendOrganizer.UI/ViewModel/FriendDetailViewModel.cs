using MyFriendOrganizer.UI.Data;
using MyFriendOrganizer.UI.Event;
using System.Threading.Tasks;
using Prism.Events;
using System.Windows.Input;
using Prism.Commands;
using MyFriendOrganizer.UI.Wrapper;

namespace MyFriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendDataService _dataService;
        private IEventAggregator _eventAggregator;
        private FriendWrapper _selectedFriend;

        public FriendDetailViewModel(IFriendDataService friendDataService, IEventAggregator eventAggregator)
        {
            _dataService = friendDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Subscribe(OnOpenFriendDetailView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int friendId)
        {
            var friend = await _dataService.GetByIdAsync(friendId);
            SelectedFriend = new FriendWrapper(friend);

            // Deze functie wordt ook uitgevoerd als er properties changen
            SelectedFriend.PropertyChanged += (s, e) =>
            {
                //Als er errors zijn 
                if (e.PropertyName == nameof(SelectedFriend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged(); // wordt hier voor 1x op true gezet
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged(); // Als er errors zijn wordt true => false; Als er geen errors zijn wordt dit true
        }

        public FriendWrapper SelectedFriend
        {
            get { return _selectedFriend; }
            set
            {
                _selectedFriend = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; } // Zodat XAML eraan kan

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync(SelectedFriend.Model);
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(new AfterFriendSavedEventArgs
            {
                Id = SelectedFriend.Id,
                DisplayMember = SelectedFriend.FirstName + " " + SelectedFriend.LastName
            });
        }

        //Can't save errors if the friend has errors
        private bool OnSaveCanExecute()
        {
            //TODO: Check in addition if friend has changes
            return SelectedFriend != null && !SelectedFriend.HasErrors;
        }

        private async void OnOpenFriendDetailView(int friendId)
        {
            await LoadAsync(friendId);
        }
    }
}
