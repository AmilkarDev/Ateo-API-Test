using Ateo_API.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Ateo_API.Models
{
    public class AteoContext :DbContext
    {
        public AteoContext(DbContextOptions<AteoContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Person> Persons { get; set; }
    }
}
