﻿using MyFriendOrganizer.UI.Event;
using System.Threading.Tasks;
using Prism.Events;
using System.Windows.Input;
using Prism.Commands;
using MyFriendOrganizer.UI.Wrapper;
using MyFriendOrganizer.UI.Data.Repositories;
using MyFriendOrganizer.Model;
using System;
using MyFriendOrganizer.UI.View.Services;
using MyFriendOrganizer.UI.Data.Lookups;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyFriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendRepository _friendRepository;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
        private IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;
        private FriendWrapper _selectedFriend;
        private FriendPhoneNumberWrapper _selectedPhoneNumber;
        private bool _hasChanges;

        public FriendDetailViewModel(IFriendRepository friendRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
        {
            _friendRepository = friendRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _programmingLanguageLookupDataService = programmingLanguageLookupDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
        }

        public async Task LoadAsync(int? friendId) // int? nullable
        {
            var friend = friendId.HasValue ? await _friendRepository.GetByIdAsync(friendId.Value) : CreateNewFriend();

            InitializeSelectedFriend(friend);
            InitializeSelectedPhoneNumber(friend.PhoneNumbers);

            await LoadProgrammingLanguagesLookupAsync();
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

        public FriendPhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand AddPhoneNumberCommand { get; set; }

        public ICommand RemovePhoneNumberCommand { get; set; }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }

        public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; set; }

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

        private bool OnSaveCanExecute()
        {
            return SelectedFriend != null // Als er een friend selected is
                && !SelectedFriend.HasErrors // Als de selected friend geen errors heeft
                && PhoneNumbers.All(phoneNumber => !phoneNumber.HasErrors) // Als er geen phonenumbers zijn met errors
                && HasChanges; // Als er changes zijn
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            _friendRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _friendRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        //Only able to remove a phoneNumber if there's one selected
        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            SelectedFriend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = ""; //Trigger validation: phonenumbe can't be empty
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _friendRepository.Add(friend);
            return friend;
        }

        private void InitializeSelectedFriend(Friend friend)
        {
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
                //Trick to trigger validation, mag niet null zijn
                SelectedFriend.FirstName = "";
            }
        }

        private void InitializeSelectedPhoneNumber(ICollection<FriendPhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (var friendPhoneNumber in phoneNumbers)
            {
                var wrapper = new FriendPhoneNumberWrapper(friendPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            }
        }

        private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _friendRepository.HasChanges();
            }
            if (e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors)) //Can't save friend if the phone number has an error
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = " - " });
            var lookup = await _programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }
    }
}
