using System.Threading.Tasks;

namespace MyFriendOrganizer.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? id); //int? nullable
        bool HasChanges { get; }
    }
}