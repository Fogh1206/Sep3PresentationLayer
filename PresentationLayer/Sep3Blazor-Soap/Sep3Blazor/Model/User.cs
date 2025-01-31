﻿using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Sep3Blazor.Model
{
    public class User
    {
        public int id { get; set; }
        [Required] [StringLength(20)] 
        public String firstName { get; set; }
        [Required] [StringLength(20)] 
        public String lastName { get; set; }
        [Required] [StringLength(15)] 
        public String username { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 7,
            ErrorMessage = "Last name should be between 7 and 15 characters")]
        public String password { get; set; }
        
        public User(int id, string firstName, string lastName, string username, string password)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.username = username;
            this.password = password;
        }

    }
}