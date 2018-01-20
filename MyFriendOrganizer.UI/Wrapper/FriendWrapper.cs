using MyFriendOrganizer.Model;
using System;
using System.Collections.Generic;

namespace MyFriendOrganizer.UI.Wrapper
{
    // ModelWrapper regelt custom errors & validatie
    public class FriendWrapper : ModelWrapper<Friend>
    {
        public FriendWrapper(Friend model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } }

        public string FirstName
        {
            get { return GetValue<string>(); } // CallerMemberName = Firstname en verwacht een string
            set
            {
                SetValue<string>(value);
            }
        }

        public string LastName
        {
            get { return GetValue<string>(); ; }
            set { SetValue<string>(value); }
        }

        public string Email
        {
            get { return GetValue<string>(); ; }
            set { SetValue<string>(value); }
        }

        //Here come custom error messages
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Robots are not valid friends";
                    }
                    break;
            }
        }
    }
}
