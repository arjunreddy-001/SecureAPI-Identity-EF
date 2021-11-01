using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureAPI.Models.DTOs
{
    public class AuthUserDTO
    {
        public AuthUserDTO(string fullname, string email, string userName, DateTime dateCreated)
        {
            Name = fullname;
            Email = email;
            UserName = userName;
            DateCreated = dateCreated;
        }

        public string Name { get; set; }
        
        public string Email { get; set; }

        public string UserName { get; set; }

        public DateTime DateCreated { get; set; }

        public string Token { get; set; }
    }
}