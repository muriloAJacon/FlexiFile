using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlexiFile.API.Migrations
{
    /// <inheritdoc />
    public partial class UserStorageUsed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "hard_storage_limit",
                schema: "FlexiFile",
                table: "User",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "storage_used",
                schema: "FlexiFile",
                table: "User",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hard_storage_limit",
                schema: "FlexiFile",
                table: "User");

            migrationBuilder.DropColumn(
                name: "storage_used",
                schema: "FlexiFile",
                table: "User");
        }
    }
}
