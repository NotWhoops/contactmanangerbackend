using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogapibvh2.Models.DTO;
using blogapijlmv2.Models;
using Microsoft.EntityFrameworkCore;

namespace blogapibvh2.Services.Context
{
public class DataContext : DbContext
{
        
public DataContext(DbContextOptions options) : base(options)
{
    
}

public DbSet<UserModel> UserInfo {get; set;}
public DbSet<UserIdDTO> UserIdInfo {get; set;}


    }
}