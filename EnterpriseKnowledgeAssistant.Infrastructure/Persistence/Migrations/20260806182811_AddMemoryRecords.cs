using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace EnterpriseKnowledgeAssistant.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMemoryRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemoryRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Embedding = table.Column<Vector>(type: "vector(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemoryRecords");
        }
    }
}
