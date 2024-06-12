using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlexiFile.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFileConversionResultType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "type_id",
                schema: "FlexiFile",
                table: "FileConversionResult",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FileConversionResult_type_id",
                schema: "FlexiFile",
                table: "FileConversionResult",
                column: "type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileConversionResult_FileType_type_id",
                schema: "FlexiFile",
                table: "FileConversionResult",
                column: "type_id",
                principalSchema: "FlexiFile",
                principalTable: "FileType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileConversionResult_FileType_type_id",
                schema: "FlexiFile",
                table: "FileConversionResult");

            migrationBuilder.DropIndex(
                name: "IX_FileConversionResult_type_id",
                schema: "FlexiFile",
                table: "FileConversionResult");

            migrationBuilder.DropColumn(
                name: "type_id",
                schema: "FlexiFile",
                table: "FileConversionResult");
        }
    }
}
