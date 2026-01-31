using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedTechnologies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Technologies_ProjectId",
                table: "Technologies");

            migrationBuilder.DropIndex(
                name: "IX_Technologies_ProjectId_IsDeleted",
                table: "Technologies");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Technologies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Technologies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Technologies_ProjectId",
                table: "Technologies",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Technologies_ProjectId_IsDeleted",
                table: "Technologies",
                columns: new[] { "ProjectId", "IsDeleted" });
        }
    }
}
