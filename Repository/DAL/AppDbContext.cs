using Microsoft.EntityFrameworkCore;

namespace Repository.DAL
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }
}
