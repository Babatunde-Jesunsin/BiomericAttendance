using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prunedge_User_Administration_Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prunedge_User_Administration.Data.Entities
{
    public class AdminstrationDbContext : IdentityDbContext<ApplicationUser>
    {
        public AdminstrationDbContext(DbContextOptions<AdminstrationDbContext> options) : base(options) { }
        public DbSet<Courses> Courses { get; set; }

    }
}
