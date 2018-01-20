using System.Collections.Generic;
using System.Threading.Tasks;
using MyFriendOrganizer.Model;

namespace MyFriendOrganizer.UI.Data
{
    public interface IFriendLookupDataService
    {
        Task<List<LookupItem>> GetFriendLookupAsync();
    }
}