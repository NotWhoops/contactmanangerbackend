using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using blogapibvh2.Models.DTO;
using blogapibvh2.Services.Context;
using blogapijlmv2.Models;
using blogapijlmv2.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace blogapijlmv2.Services
{
    public class UserService : ControllerBase
    {
        private readonly DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;
        }

        public bool DoesUserExist(string username)
        {
            return _context.UserInfo.SingleOrDefault(user => user.Username == username) != null;
        }

        public bool AddUser(CreateAccountDTO userToAdd)
        {
            bool result = false;

            if (userToAdd.Username != null && !DoesUserExist(userToAdd.Username))
            {
                UserModel newUser = new UserModel();

                var HashedPassword = HashPassword(userToAdd.Password);

                newUser.Id = userToAdd.Id;
                newUser.Username = userToAdd.Username;

                newUser.Salt = HashedPassword.Salt;
                newUser.Hash = HashedPassword.Hash;

                _context.Add(newUser);

                result = _context.SaveChanges() != 0;

            }
            return result;
        }

        public PasswordDTO HashPassword(string? password)
        {
            PasswordDTO newHashedPassword = new PasswordDTO();

            byte[] SaltBytes = new byte[64];

            var provider = RandomNumberGenerator.Create();
            provider.GetNonZeroBytes(SaltBytes);

            var Salt = Convert.ToBase64String(SaltBytes);

            var rfc2898DerviceBytes = new Rfc2898DeriveBytes(password ?? "", SaltBytes, 10000,
            HashAlgorithmName.SHA256);

            var Hash = Convert.ToBase64String(rfc2898DerviceBytes.GetBytes(256));

            newHashedPassword.Salt = Salt;
            newHashedPassword.Hash = Hash;

            return newHashedPassword;
        }

        public bool verifyUserPassword(string? Password, string? StoredHash, string? StoredSalt)
        {
            if (StoredSalt == null)
            {
                return false;
            }

            var SaltBytes = Convert.FromBase64String(StoredSalt);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(Password ?? "", SaltBytes, 10000,
            HashAlgorithmName.SHA256);

            var newHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            return newHash == StoredHash;

        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            return _context.UserInfo;
        }

        public UserModel GetAllUserDataByUsername(string username)
        {
            return _context.UserInfo.FirstOrDefault(user => user.Username == username);
        }
        public IActionResult Login(LoginDTO user)
        {
            IActionResult result = Unauthorized();
           
            if (DoesUserExist(user.Username))
            {
                UserModel foundUser = GetAllUserDataByUsername(user.Username);
                if(verifyUserPassword(user.Password, foundUser.Hash, foundUser.Salt))
                {
                    

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("supersupersupersuperdupersecurekey@34456789"));
    
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256); 


                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5181",
                    audience: "https://localhost:5181",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signingCredentials
                );
                

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

      
                result = Ok(new {Token = tokenString});
                }
            }

            return result;
        }

        public UserIdDTO GetUserIdDTOByUserName(string username)
        {
            return _context.UserIdInfo.FirstOrDefault(UserIdDTO => UserIdDTO.PublisherName == username);
        }

        public UserModel GetUserByUsername(string username)
        {
            return _context.UserInfo.SingleOrDefault(user => user.Username ==  username);
        }

        public bool DeleteUser(string userToDelete)
        {
            UserModel foundUser = GetUserByUsername(userToDelete);
            bool result = false;

            if(foundUser != null)
            {
                foundUser.Username = userToDelete;
                _context.Remove(foundUser);

                result = _context.SaveChanges() != 0;

            }
            return result;
        }

        public UserModel GetUserById(int id)
        {
            return _context.UserInfo.SingleOrDefault(user => user.Id == id);
        }

        public bool UpdateUser(int id, string username)
        {
            UserModel foundUser = GetUserById(id);
            bool result = false;
            if(foundUser != null)
            {
                foundUser.Username = username;
                _context.Update(foundUser);
                result = _context.SaveChanges() != 0;

            }
            return result;
        }
    }
}