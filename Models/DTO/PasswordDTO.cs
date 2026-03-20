using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogapijlmv2.Models.DTO
{
    public class PasswordDTO
    {
        public string Salt { get; set; }
        public string Hash { get; set; }
    }
}