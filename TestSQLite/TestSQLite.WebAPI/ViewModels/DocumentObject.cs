using System;
using TestSQLite.Repository.SQLite.Entities;

namespace TestSQLite.WebAPI.ViewModels
{
    public class DocumentObjectViewModel
    {
        public Guid UserId { get; set; }
        public string JsonObject { get; set; }

        public DocumentObject GetEntity() =>
            new DocumentObject
            {
                Id = Guid.NewGuid(),
                JsonObject = this.JsonObject,
                Created = DateTime.Now,
                Updated = null,
                UserId = this.UserId,
                Active = true
            };
    }
}
