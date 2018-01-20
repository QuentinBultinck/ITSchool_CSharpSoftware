using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyFriendOrganizer.UI.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //Protected = subclasses kunnen deze functie gebruiken; Virtual = subclasses kunnen het overriden
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null) //CallerMemberName vb: SelectedFriend changed => propertyName = SelectedFriend
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
