using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubgAPI
{
    public class User
    {
        private string password;
        [JsonIgnore]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public string Password { get => password; set => password = value; }
        public string Token { get; set; }

        //public User(string userName, string password, DateTime creationDate)
        //{
        //    UserName = userName;
        //    Password = Encrypt(password);
        //    CreationDate = creationDate;
        //    Token = Encrypt(UserName + Password);
        //}



        //public static bool Authorize(string Token)
        //{
        //    using (var context = new FoodleContext())
        //    {
        //        var user = context.Users.Where(u => u.Token == Token).FirstOrDefault();
        //        if (user != null)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}
