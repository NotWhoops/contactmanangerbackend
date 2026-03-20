using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogapibvh2.Models.DTO;
using blogapijlmv2.Models;
using blogapijlmv2.Models.DTO;
using blogapijlmv2.Services;
using Microsoft.AspNetCore.Mvc;

namespace blogapijlmv2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _data;
        public UserController(UserService dataFromService)
        {
            _data = dataFromService;
        }

        [HttpPost("AddUser")]
        public bool AddUser(CreateAccountDTO UserToAdd)
        {
            return _data.AddUser(UserToAdd);
        }


        [HttpGet("GetAllUsers")]
        public IEnumerable<UserModel> GetAllUsers()
        {
            return _data.GetAllUsers();
        }


        [HttpGet("GetUserByUserName")]
        public UserIdDTO GetUserDTOUserName(string username)
        {
            return _data.GetUserIdDTOByUserName(username);
        }


        [HttpPost("Login")]
        public IActionResult Login ( [FromBody] LoginDTO user)
        {
            return _data.Login(user);
        }

     
        [HttpPost("DeleteUser/{userToDelete}")]
        public bool DeleteUser(string userToDelete)
        {
           return _data.DeleteUser(userToDelete);
        }

        [HttpPut("UpdateUser")]
        public bool UpdateUser(int id, string username)
        {
            return _data.UpdateUser(id, username);
        }
    }
}