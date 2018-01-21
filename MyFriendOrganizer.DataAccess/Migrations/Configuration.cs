namespace MyFriendOrganizer.DataAccess.Migrations
{
    using MyFriendOrganizer.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyFriendOrganizer.DataAccess.FriendOrganizerDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyFriendOrganizer.DataAccess.FriendOrganizerDBContext context)
        {
            context.Friends.AddOrUpdate(f => f.FirstName,  //Als firstname bestaat => updaten else add
                new Friend { FirstName = "Thomas", LastName = "Huber" },
                new Friend { FirstName = "Andreas", LastName = "Boehler" },
                new Friend { FirstName = "Julia", LastName = "Huber" },
                new Friend { FirstName = "Chrissi", LastName = "Egin" }
            );

            context.ProgrammingsLanguages.AddOrUpdate(pl => pl.Name,
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "Typescript" },
                new ProgrammingLanguage { Name = "F#" },
                new ProgrammingLanguage { Name = "Swift" },
                new ProgrammingLanguage { Name = "Java" }
            );

            context.SaveChanges(); //Zodat we hieronder de eerste Friend z'n ID kunnen nemen

            context.FriendPhoneNumbers.AddOrUpdate(ph => ph.Number,
                new FriendPhoneNumber { Number = "+49 12345678", FriendId = context.Friends.First().Id }
                );
        }
    }
}
