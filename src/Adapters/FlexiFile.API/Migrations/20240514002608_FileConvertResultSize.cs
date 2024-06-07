using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlexiFile.API.Migrations
{
    /// <inheritdoc />
    public partial class FileConvertResultSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "size",
                schema: "FlexiFile",
                table: "FileConversionResult",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "size",
                schema: "FlexiFile",
                table: "FileConversionResult");
        }
    }
}
