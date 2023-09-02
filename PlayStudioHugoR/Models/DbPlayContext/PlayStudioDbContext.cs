using Microsoft.EntityFrameworkCore;
using PlayStudioHugoR.Models.Entities;

namespace PlayStudioHugoR.Models.DbPlayContext
{
    public class PlayStudioDbContext : DbContext
    {
        internal string dbschema;
        private readonly IConfiguration _configuration;

        public PlayStudioDbContext(DbContextOptions<PlayStudioDbContext> options, IConfiguration configuration)
            :base(options)
        {
            _configuration = configuration;
            dbschema = _configuration.GetValue<string>("AppSetting:DBSchema");
        }

        public DbSet<UsersModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersModel>(entity =>
            {
                entity.Property(e => e.id).HasColumnName("id");
            });
                modelBuilder.HasDefaultSchema(dbschema);
        }

    }
}
