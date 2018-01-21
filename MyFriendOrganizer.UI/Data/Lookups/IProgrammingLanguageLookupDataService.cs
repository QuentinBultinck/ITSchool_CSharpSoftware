using System.Collections.Generic;
using System.Threading.Tasks;
using MyFriendOrganizer.Model;

namespace MyFriendOrganizer.UI.Data.Lookups
{
    public interface IProgrammingLanguageLookupDataService
    {
        Task<List<LookupItem>> GetProgrammingLanguageLookupAsync();
    }
}