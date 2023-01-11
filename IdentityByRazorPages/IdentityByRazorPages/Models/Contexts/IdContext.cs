using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityByRazorPages.Models.Contexts;

public class IdContext : IdentityDbContext<IdentityUser>
{
    public IdContext(DbContextOptions<IdContext> options)
        : base(options)
    {
    }
}