using MyFriendOrganizer.UI.Event;
using System.Threading.Tasks;
using Prism.Events;
using System.Windows.Input;
using Prism.Commands;
using MyFriendOrganizer.UI.Wrapper;
using MyFriendOrganizer.UI.Data.Repositories;
using MyFriendOrganizer.Model;
using System;
using MyFriendOrganizer.UI.View.Services;

namespace MyFriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendRepository _friendRepository;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
        private FriendWrapper _selectedFriend;
        private bool _hasChanges;

        public FriendDetailViewModel(IFriendRepository friendRepository, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _friendRepository = friendRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        public async Task LoadAsync(int? friendId) // int? nullable
        {
            var friend = friendId.HasValue ? await _friendRepository.GetByIdAsync(friendId.Value) : CreateNewFriend();
            SelectedFriend = new FriendWrapper(friend);

            // Deze functie wordt ook uitgevoerd als er properties changen
            SelectedFriend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _friendRepository.HasChanges();
                }
                //Als er errors zijn 
                if (e.PropertyName == nameof(SelectedFriend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged(); // wordt hier voor 1x op true gezet
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged(); // Als er errors zijn wordt true => false; Als er geen errors zijn wordt dit true

            if (SelectedFriend.Id == 0)
            {
                //Trick to trigger validation
                SelectedFriend.FirstName = "";
            }
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

        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the friend {SelectedFriend.FirstName} {SelectedFriend.LastName}", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _friendRepository.Remove(SelectedFriend.Model);
                await _friendRepository.SaveAsync();
                _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(SelectedFriend.Id);
            }
        }

        private async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();
            HasChanges = _friendRepository.HasChanges();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(new AfterFriendSavedEventArgs
            {
                Id = SelectedFriend.Id,
                DisplayMember = SelectedFriend.FirstName + " " + SelectedFriend.LastName
            });
        }

        //Can't save errors if the friend has errors
        private bool OnSaveCanExecute()
        {
            return SelectedFriend != null && !SelectedFriend.HasErrors && HasChanges;
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _friendRepository.Add(friend);
            return friend;
        }
    }
}
