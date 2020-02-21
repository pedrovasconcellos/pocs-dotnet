using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestSQLite.Repository.SQLite.Migrations
{
    public partial class intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentObject",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    JsonObject = table.Column<string>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentObject", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentObject");
        }
    }
}
