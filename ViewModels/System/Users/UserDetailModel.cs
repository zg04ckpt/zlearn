﻿
using Data.Enums;
using System;

namespace ViewModels.System.Users
{
    public class UserDetailModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DayOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public string Address { get; set; }
        public string Intro { get; set; }
        public string SocialLinks { get; set; }
    }
}
