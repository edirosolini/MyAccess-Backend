// <copyright file="20211112011739_CreateTable_Users.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace MyAccess.Providers.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using MyAccess.Commons;

    public partial class CreateTable_Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                table: "Users",
                column: "EmailAddress",
                unique: true);

            var userId = Guid.NewGuid();

            migrationBuilder.InsertData(
                table: "Users",
                columns: new string[] { "Id", "LastName", "FirstName", "EmailAddress", "Password", "CreationDate", "CreatedBy", "ModificationDate", "ModifiedBy" },
                values: new object[] { userId, "Test", "Test", "test@dominio.com", PasswordHash.CreateHash("AbcD1234"), DateTime.UtcNow, userId, DateTime.UtcNow, userId });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
