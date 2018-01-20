using System.Collections.Generic;
using System.Threading.Tasks;
using MyFriendOrganizer.Model;

namespace MyFriendOrganizer.UI.Data.Lookups
{
    public interface IFriendLookupDataService
    {
        Task<List<LookupItem>> GetFriendLookupAsync();
    }
}