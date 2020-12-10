using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RazorPages.Models;

namespace RazorPages.Models
{
    public class Context : IdentityDbContext<Identity.RazorPagesUser>
    {
        public Context (DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }
        public DbSet<Identity.RazorPagesUser> UsersData { get; set; }
    }
}
