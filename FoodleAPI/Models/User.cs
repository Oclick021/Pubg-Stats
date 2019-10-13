using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PubgAPI.Models
{
    public class User
    {
        private string password;
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get => password; set => password = value; }
        public string Token { get; set; }
        public string Role { get; set; } = Models.Role.GeneralUser;

    }
}
