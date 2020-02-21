
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestSQLite.Repository.SQLite.Entities;
using TestSQLite.Repository.SQLite.Mappings;

namespace TestSQLite.Repository.SQLite
{
    public class SQLiteContext : DbContext
    {
        public DbSet<DocumentObject> DocumentObject { get; set; }
        
        public SQLiteContext(DbContextOptions<SQLiteContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DocumentObjectMapping());
            base.OnModelCreating(builder);
        }
    }
}