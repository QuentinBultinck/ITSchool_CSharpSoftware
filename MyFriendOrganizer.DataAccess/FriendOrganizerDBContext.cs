using MyFriendOrganizer.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MyFriendOrganizer.DataAccess
{
    public class FriendOrganizerDBContext : DbContext
    {
        public FriendOrganizerDBContext() : base("MyFriendOrganizerDB")
        {

        }

        //Tables
        public DbSet<Friend> Friends { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingsLanguages { get; set; }
        public DbSet<FriendPhoneNumber> FriendPhoneNumbers { get; set; }
        public DbSet<Meeting> Meetings { get; set; }

        //Hier zeg je hoe de database er moet uitzien
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); // Bij code first, tablenames zonder dat ze plural staan

            //Wij werken met DataAnnotations in de Model zelf
            //modelBuilder.Configurations.Add(new FriendConfiguration());
        }
    }

    //public class FriendConfiguration : EntityTypeConfiguration<Friend>
    //{
    //    public FriendConfiguration()
    //    {
    //        Property(f => f.FirstName)
    //            .IsRequired()
    //            .HasMaxLength(50);
    //    }
    //}
}
