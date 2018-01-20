using System.Collections.Generic;
using MyFriendOrganizer.Model;
using System.Threading.Tasks;

namespace MyFriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository
    {
        Task<Friend> GetByIdAsync(int friendId);
        Task SaveAsync();
        bool HasChanges();
        void Add(Friend friend);
        void Remove(Friend model);
    }
}