using System.Collections.Generic;
using MyFriendOrganizer.Model;
using System.Threading.Tasks;

namespace MyFriendOrganizer.UI.Data
{
    public interface IFriendDataService
    {
        Task<List<Friend>> GetAllAsync();
    }
}