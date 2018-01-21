using System.Threading.Tasks;
using System.Data.Entity;
using MyFriendOrganizer.Model;
using MyFriendOrganizer.DataAccess;

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
    }
}
