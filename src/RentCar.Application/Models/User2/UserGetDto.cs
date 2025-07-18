﻿using System.ComponentModel.DataAnnotations;

namespace RentCar.Application.Models.User2
{
    public class UserGetDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; } // Nullable DateTime

    }

}
