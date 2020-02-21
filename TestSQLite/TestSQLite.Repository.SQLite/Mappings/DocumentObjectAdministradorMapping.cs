using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestSQLite.Repository.SQLite.Entities;

namespace TestSQLite.Repository.SQLite.Mappings
{
    public class DocumentObjectMapping : IEntityTypeConfiguration<DocumentObject>
    {
        public void Configure(EntityTypeBuilder<DocumentObject> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.JsonObject).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Created).IsRequired();
            builder.Property(x => x.Updated);
            builder.Property(x => x.Active).IsRequired();
        }
    }
}
