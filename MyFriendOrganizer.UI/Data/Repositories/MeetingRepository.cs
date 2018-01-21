using System.Threading.Tasks;
using System.Data.Entity;
using MyFriendOrganizer.Model;
using MyFriendOrganizer.DataAccess;
using System.Collections.Generic;

namespace MyFriendOrganizer.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDBContext>, IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDBContext context) : base(context)
        {
        }

        public async override Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings
              .Include(m => m.Friends)
              .SingleAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Friend>> GetAllFriendsAsync()
        {
            return await Context.Set<Friend>().ToListAsync();
        }
    }
}
