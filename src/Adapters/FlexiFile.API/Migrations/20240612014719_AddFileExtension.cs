using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlexiFile.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFileExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "extension",
                schema: "FlexiFile",
                table: "FileType",
                type: "character varying",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 1,
                column: "extension",
                value: "png");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 2,
                column: "extension",
                value: "jpeg");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 3,
                column: "extension",
                value: "ico");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 4,
                column: "extension",
                value: "svg");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 5,
                column: "extension",
                value: "gif");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 6,
                column: "extension",
                value: "tiff");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 7,
                column: "extension",
                value: "mp4");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 8,
                column: "extension",
                value: "mpeg");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 9,
                column: "extension",
                value: "webm");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 10,
                column: "extension",
                value: "mkv");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 11,
                column: "extension",
                value: "mp3");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 12,
                column: "extension",
                value: "m4a");

            migrationBuilder.UpdateData(
                schema: "FlexiFile",
                table: "FileType",
                keyColumn: "id",
                keyValue: 13,
                column: "extension",
                value: "pdf");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "extension",
                schema: "FlexiFile",
                table: "FileType");
        }
    }
}
