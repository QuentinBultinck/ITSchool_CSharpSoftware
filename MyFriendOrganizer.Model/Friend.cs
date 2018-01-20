﻿using System.ComponentModel.DataAnnotations;

namespace MyFriendOrganizer.Model
{
    public class Friend
    {
        // [Key]     heeft naam Id => automatisch gezien als PK
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
    }
}
