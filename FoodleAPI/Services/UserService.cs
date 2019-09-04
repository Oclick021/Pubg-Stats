
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PubgAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PubgAPI.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        void SignToken(User user);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string username, string password)
        {
            //using (var context = new FoodleContext())
            //{
            //    var user = context.Users.Where(x => x.Username == username && x.Password == Encrypt(password)).FirstOrDefault();
            //    // return null if user not found
            //    if (user == null)
            //        return null;

            //    SignToken(user);

            //    context.Update(user);
            //    // remove password before returning
            //    user.Password = null;
            //    return user;
            //}
            return null; //just to catch the error

        }

        public void SignToken(User user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
        }
        public static async Task<User> GetByID(int id)
        {
            //using (var context = new FoodleContext())
            //{
            //    return await context.Users.FindAsync(id);
            //}

            return null; //just to catch the error
        }
        public static string Encrypt(string pass)
        {
            var hash = new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(pass));
            return Convert.ToBase64String(hash);
        }
    }
}
