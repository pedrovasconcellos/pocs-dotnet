using System;

namespace TestSQLite.Repository.SQLite.Entities
{
    public class DocumentObject
    {
        public Guid Id { get; set; }
        public string JsonObject { get; set; }
        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool Active { get; set; }
    }
}
