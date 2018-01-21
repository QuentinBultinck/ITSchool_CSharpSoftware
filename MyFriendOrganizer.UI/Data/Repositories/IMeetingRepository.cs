using MyFriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFriendOrganizer.UI.Data.Repositories
{
    public interface IMeetingRepository : IGenericRepository<Meeting>
    {
        Task<IEnumerable<Friend>> GetAllFriendsAsync();
    }
}