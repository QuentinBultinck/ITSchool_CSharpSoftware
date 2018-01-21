using MyFriendOrganizer.DataAccess;
using MyFriendOrganizer.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MyFriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private FriendOrganizerDBContext _context;

        public FriendRepository(FriendOrganizerDBContext context)
        {
            _context = context;
        }

        public void Add(Friend friend)
        {
            //Context changes
            _context.Friends.Add(friend);
        }

        public async Task<Friend> GetByIdAsync(int friendId)
        {
            return await _context.Friends
                .Include(f => f.PhoneNumbers)
                .SingleAsync(f => f.Id == friendId);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Remove(Friend model)
        {
            _context.Friends.Remove(model);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            _context.FriendPhoneNumbers.Remove(model); 
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
