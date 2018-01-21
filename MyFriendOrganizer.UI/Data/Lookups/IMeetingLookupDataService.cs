using MyFriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFriendOrganizer.UI.Data.Lookups
{
    public interface IMeetingLookupDataService
    {
        Task<List<LookupItem>> GetMeetingLookupAsync();
    }
}
