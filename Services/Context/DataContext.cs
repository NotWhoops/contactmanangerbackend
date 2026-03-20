using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


    }
}