using Microsoft.EntityFrameworkCore;
using Sangiaodich.Model;

namespace Sangiaodich.DBContext
{
    public class DBSetIdentityDBContext :DbContext
    {
        public DBSetIdentityDBContext(DbContextOptions<DBSetIdentityDBContext> options) : base(options)
        {

        }
        protected DBSetIdentityDBContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data\\DataSQL.db;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


        public virtual DbSet<MCK> MCK { get; set; }
    }
}
