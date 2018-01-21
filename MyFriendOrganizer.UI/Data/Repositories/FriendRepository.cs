using MyFriendOrganizer.DataAccess;
using MyFriendOrganizer.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MyFriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : GenericRepository<Friend, FriendOrganizerDBContext>, IFriendRepository
    {
        public FriendRepository(FriendOrganizerDBContext context) : base(context)
        {
        }

        public override async Task<Friend> GetByIdAsync(int friendId)
        {
            return await Context.Friends
                .Include(f => f.PhoneNumbers)
                .SingleAsync(f => f.Id == friendId);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.FriendPhoneNumbers.Remove(model);
        }
    }
}
