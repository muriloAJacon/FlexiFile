using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlexiFile.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "FlexiFile");

            migrationBuilder.CreateTable(
                name: "FileType",
                schema: "FlexiFile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying", nullable: false),
                    mime_type = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FileType_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "FlexiFile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    email = table.Column<string>(type: "character varying", nullable: false),
                    password = table.Column<string>(type: "character varying", nullable: false),
                    access_level = table.Column<int>(type: "integer", nullable: false),
                    approved = table.Column<bool>(type: "boolean", nullable: false),
                    approved_at = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: true),
                    approved_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_update_date = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: false),
                    storage_limit = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_User", x => x.id);
                    table.ForeignKey(
                        name: "fk_User_User_approvedby",
                        column: x => x.approved_by_user_id,
                        principalSchema: "FlexiFile",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_User_User_createdby",
                        column: x => x.created_by_user_id,
                        principalSchema: "FlexiFile",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FileTypeConversion",
                schema: "FlexiFile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false, comment: "(\"Conversion\",\"Processing\")"),
                    from_type_id = table.Column<int>(type: "integer", nullable: false),
                    to_type_id = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    handler_class_name = table.Column<string>(type: "character varying", nullable: false),
                    min_number_files = table.Column<int>(type: "integer", nullable: true),
                    max_number_files = table.Column<int>(type: "integer", nullable: true),
                    model_class_name = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_FileTypeConversion", x => x.id);
                    table.ForeignKey(
                        name: "fk_FileTypeConversion_FileType_from_type",
                        column: x => x.from_type_id,
                        principalSchema: "FlexiFile",
                        principalTable: "FileType",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_FileTypeConversion_FileType_to_type",
                        column: x => x.to_type_id,
                        principalSchema: "FlexiFile",
                        principalTable: "FileType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "File",
                schema: "FlexiFile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_id = table.Column<int>(type: "integer", nullable: false),
                    owned_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    original_name = table.Column<string>(type: "character varying", nullable: false),
                    submitted_at = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: false),
                    finished_upload = table.Column<bool>(type: "boolean", nullable: false),
                    finished_upload_at = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_File", x => x.id);
                    table.ForeignKey(
                        name: "File_FileType_id_fk",
                        column: x => x.type_id,
                        principalSchema: "FlexiFile",
                        principalTable: "FileType",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_File_User",
                        column: x => x.owned_by_user_id,
                        principalSchema: "FlexiFile",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                schema: "FlexiFile",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    value = table.Column<string>(type: "character varying", nullable: false),
                    last_update_date = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: true),
                    updated_by_user_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_Setting", x => x.id);
                    table.ForeignKey(
                        name: "fk_Setting_User",
                        column: x => x.updated_by_user_id,
                        principalSchema: "FlexiFile",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserLoginAudit",
                schema: "FlexiFile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    successful = table.Column<bool>(type: "boolean", nullable: false),
                    source_ip = table.Column<string>(type: "character varying", nullable: false),
                    source_user_agent = table.Column<string>(type: "character varying", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_UserLoginAudit", x => x.id);
                    table.ForeignKey(
                        name: "fk_UserLoginAudit_User",
                        column: x => x.user_id,
                        principalSchema: "FlexiFile",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserRefreshToken",
                schema: "FlexiFile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_UserRefreshToken", x => x.id);
                    table.ForeignKey(
                        name: "fk_UserRefreshToken_User",
                        column: x => x.user_id,
                        principalSchema: "FlexiFile",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FileConversion",
                schema: "FlexiFile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_type_conversion_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, comment: "(\"InQueue\",\"InProgress\",\"Completed\",\"Failed\")"),
                    percentage_complete = table.Column<double>(type: "double precision", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: false),
                    last_update_date = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: false),
                    extra_info = table.Column<JsonElement>(type: "json", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FileConversion_pk", x => x.id);
                    table.ForeignKey(
                        name: "fk_FileConversion_FileTypeConversion",
                        column: x => x.file_type_conversion_id,
                        principalSchema: "FlexiFile",
                        principalTable: "FileTypeConversion",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_FileConversion_User",
                        column: x => x.user_id,
                        principalSchema: "FlexiFile",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FileConversionOrigin",
                schema: "FlexiFile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_conversion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    extra_info = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_FileConversionOrigin", x => x.id);
                    table.ForeignKey(
                        name: "fk_FileConversionOrigin_File",
                        column: x => x.file_id,
                        principalSchema: "FlexiFile",
                        principalTable: "File",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_FileConversionOrigin_FileConversion",
                        column: x => x.file_conversion_id,
                        principalSchema: "FlexiFile",
                        principalTable: "FileConversion",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FileConversionResult",
                schema: "FlexiFile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_conversion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, scale: 0, nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FileConversionResult_pk", x => x.id);
                    table.ForeignKey(
                        name: "FileConversionResult_FileConversion_id_fk",
                        column: x => x.file_conversion_id,
                        principalSchema: "FlexiFile",
                        principalTable: "FileConversion",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                schema: "FlexiFile",
                table: "FileType",
                columns: new[] { "id", "description", "mime_type" },
                values: new object[,]
                {
                    { 1, "PNG", "image/png" },
                    { 2, "JPEG", "image/jpeg" },
                    { 3, "ICO", "image/vnd.microsoft.icon" },
                    { 4, "SVG", "image/svg+xml" },
                    { 5, "GIF", "image/gif" },
                    { 6, "TIFF", "image/tiff" },
                    { 7, "MP4", "video/mp4" },
                    { 8, "MPEG", "video/mpeg" },
                    { 9, "WEBM", "video/webm" },
                    { 10, "MKV", "video/x-matroska" },
                    { 11, "MP3", "audio/mpeg" },
                    { 12, "M4A", "audio/mp4" },
                    { 13, "PDF", "application/pdf" }
                });

            migrationBuilder.InsertData(
                schema: "FlexiFile",
                table: "Setting",
                columns: new[] { "id", "last_update_date", "updated_by_user_id", "value" },
                values: new object[,]
                {
                    { "ALLOW_ANONYMOUS_REGISTER", null, null, "False" },
                    { "GLOBAL_MAXIMUM_FILE_SIZE", null, null, "0" }
                });

            migrationBuilder.InsertData(
                schema: "FlexiFile",
                table: "FileTypeConversion",
                columns: new[] { "id", "description", "from_type_id", "handler_class_name", "is_active", "max_number_files", "min_number_files", "model_class_name", "to_type_id", "type" },
                values: new object[,]
                {
                    { 1, "Converts PNG to JPEG", 1, "IConvertImageService", true, 1, null, null, 2, "1" },
                    { 2, "Converts PNG to ICO", 1, "IConvertImageService", true, 1, null, null, 3, "1" },
                    { 3, "Converts PNG to SVG", 1, "IConvertImageService", true, 1, null, null, 4, "1" },
                    { 4, "Converts MKV to MP4", 10, "IConvertVideoService", true, 1, null, null, 7, "1" },
                    { 5, "Converts MP3 to M4A", 11, "IConvertAudioService", true, 1, null, null, 12, "1" },
                    { 6, "Converts MP4 to MP3", 7, "IConvertVideoService", true, 1, null, null, 11, "1" },
                    { 7, "Converts PNG to GIF", 1, "IConvertImageService", true, 1, null, null, 5, "1" },
                    { 8, "Converts PNG to TIFF", 1, "IConvertImageService", true, 1, null, null, 6, "1" },
                    { 9, "Converts JPEG to PNG", 2, "IConvertImageService", true, 1, null, null, 1, "1" },
                    { 10, "Converts JPEG to ICO", 2, "IConvertImageService", true, 1, null, null, 3, "1" },
                    { 11, "Converts JPEG to SVG", 2, "IConvertImageService", true, 1, null, null, 4, "1" },
                    { 12, "Converts JPEG to GIF", 2, "IConvertImageService", true, 1, null, null, 5, "1" },
                    { 13, "Converts JPEG to TIFF", 2, "IConvertImageService", true, 1, null, null, 6, "1" },
                    { 14, "Converts ICO to PNG", 3, "IConvertImageService", true, 1, null, null, 1, "1" },
                    { 15, "Converts ICO to JPEG", 3, "IConvertImageService", true, 1, null, null, 2, "1" },
                    { 16, "Converts ICO to SVG", 3, "IConvertImageService", true, 1, null, null, 4, "1" },
                    { 17, "Converts ICO to GIF", 3, "IConvertImageService", true, 1, null, null, 5, "1" },
                    { 18, "Converts ICO to TIFF", 3, "IConvertImageService", true, 1, null, null, 6, "1" },
                    { 19, "Converts SVG to PNG", 4, "IConvertImageService", true, 1, null, null, 1, "1" },
                    { 20, "Converts SVG to JPEG", 4, "IConvertImageService", true, 1, null, null, 2, "1" },
                    { 21, "Converts SVG to ICO", 4, "IConvertImageService", true, 1, null, null, 3, "1" },
                    { 22, "Converts SVG to GIF", 4, "IConvertImageService", true, 1, null, null, 5, "1" },
                    { 23, "Converts SVG to TIFF", 4, "IConvertImageService", true, 1, null, null, 6, "1" },
                    { 24, "Converts GIF to PNG", 5, "IConvertImageService", true, 1, null, null, 1, "1" },
                    { 25, "Converts GIF to JPEG", 5, "IConvertImageService", true, 1, null, null, 2, "1" },
                    { 26, "Converts GIF to ICO", 5, "IConvertImageService", true, 1, null, null, 3, "1" },
                    { 27, "Converts GIF to SVG", 5, "IConvertImageService", true, 1, null, null, 4, "1" },
                    { 28, "Converts GIF to TIFF", 5, "IConvertImageService", true, 1, null, null, 6, "1" },
                    { 29, "Converts TIFF to PNG", 6, "IConvertImageService", true, 1, null, null, 1, "1" },
                    { 30, "Converts TIFF to JPEG", 6, "IConvertImageService", true, 1, null, null, 2, "1" },
                    { 31, "Converts TIFF to ICO", 6, "IConvertImageService", true, 1, null, null, 3, "1" },
                    { 32, "Converts TIFF to SVG", 6, "IConvertImageService", true, 1, null, null, 4, "1" },
                    { 33, "Converts TIFF to GIF", 6, "IConvertImageService", true, 1, null, null, 5, "1" },
                    { 34, "Converts MP4 to MPEG", 7, "IConvertVideoService", true, 1, null, null, 8, "1" },
                    { 35, "Converts MP4 to WEBM", 7, "IConvertVideoService", true, 1, null, null, 9, "1" },
                    { 36, "Converts MP4 to MKV", 7, "IConvertVideoService", true, 1, null, null, 10, "1" },
                    { 37, "Converts MPEG to MP4", 8, "IConvertVideoService", true, 1, null, null, 7, "1" },
                    { 38, "Converts MPEG to WEBM", 8, "IConvertVideoService", true, 1, null, null, 9, "1" },
                    { 39, "Converts MPEG to MKV", 8, "IConvertVideoService", true, 1, null, null, 10, "1" },
                    { 40, "Converts WEBM to MP4", 9, "IConvertVideoService", true, 1, null, null, 7, "1" },
                    { 41, "Converts WEBM to MPEG", 9, "IConvertVideoService", true, 1, null, null, 8, "1" },
                    { 42, "Converts WEBM to MKV", 9, "IConvertVideoService", true, 1, null, null, 10, "1" },
                    { 43, "Converts MKV to MPEG", 10, "IConvertVideoService", true, 1, null, null, 8, "1" },
                    { 44, "Converts MKV to WEBM", 10, "IConvertVideoService", true, 1, null, null, 9, "1" },
                    { 45, "Converts MPEG to MP3", 8, "IConvertVideoService", true, 1, null, null, 11, "1" },
                    { 46, "Converts WEBM to MP3", 9, "IConvertVideoService", true, 1, null, null, 11, "1" },
                    { 47, "Converts MKV to MP3", 10, "IConvertVideoService", true, 1, null, null, 11, "1" },
                    { 48, "Converts MP4 to M4A", 7, "IConvertVideoService", true, 1, null, null, 12, "1" },
                    { 49, "Converts MPEG to M4A", 8, "IConvertVideoService", true, 1, null, null, 12, "1" },
                    { 50, "Converts WEBM to M4A", 9, "IConvertVideoService", true, 1, null, null, 12, "1" },
                    { 51, "Converts MKV to M4A", 10, "IConvertVideoService", true, 1, null, null, 12, "1" },
                    { 52, "Converts M4A to MP3", 12, "IConvertAudioService", true, 1, null, null, 11, "1" },
                    { 53, "Splits one PDF file into multiple files", 13, "ISplitDocumentService", true, 1, null, null, null, "2" },
                    { 54, "Merges multiple PDF files into one file", 13, "IMergeDocumentService", true, null, 2, null, null, "2" },
                    { 55, "Rearranges the pages of a PDF file", 13, "IRearrangeDocumentService", true, 1, null, "RearrangeDocumentParameters", null, "2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_owned_by_user_id",
                schema: "FlexiFile",
                table: "File",
                column: "owned_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_File_type_id",
                schema: "FlexiFile",
                table: "File",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileConversion_file_type_conversion_id",
                schema: "FlexiFile",
                table: "FileConversion",
                column: "file_type_conversion_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileConversion_user_id",
                schema: "FlexiFile",
                table: "FileConversion",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileConversionOrigin_file_conversion_id",
                schema: "FlexiFile",
                table: "FileConversionOrigin",
                column: "file_conversion_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileConversionOrigin_file_id",
                schema: "FlexiFile",
                table: "FileConversionOrigin",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileConversionResult_file_conversion_id",
                schema: "FlexiFile",
                table: "FileConversionResult",
                column: "file_conversion_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileTypeConversion_from_type_id",
                schema: "FlexiFile",
                table: "FileTypeConversion",
                column: "from_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileTypeConversion_to_type_id",
                schema: "FlexiFile",
                table: "FileTypeConversion",
                column: "to_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_updated_by_user_id",
                schema: "FlexiFile",
                table: "Setting",
                column: "updated_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_approved_by_user_id",
                schema: "FlexiFile",
                table: "User",
                column: "approved_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_created_by_user_id",
                schema: "FlexiFile",
                table: "User",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginAudit_user_id",
                schema: "FlexiFile",
                table: "UserLoginAudit",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshToken_user_id",
                schema: "FlexiFile",
                table: "UserRefreshToken",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileConversionOrigin",
                schema: "FlexiFile");

            migrationBuilder.DropTable(
                name: "FileConversionResult",
                schema: "FlexiFile");

            migrationBuilder.DropTable(
                name: "Setting",
                schema: "FlexiFile");

            migrationBuilder.DropTable(
                name: "UserLoginAudit",
                schema: "FlexiFile");

            migrationBuilder.DropTable(
                name: "UserRefreshToken",
                schema: "FlexiFile");

            migrationBuilder.DropTable(
                name: "File",
                schema: "FlexiFile");

            migrationBuilder.DropTable(
                name: "FileConversion",
                schema: "FlexiFile");

            migrationBuilder.DropTable(
                name: "FileTypeConversion",
                schema: "FlexiFile");

            migrationBuilder.DropTable(
                name: "User",
                schema: "FlexiFile");

            migrationBuilder.DropTable(
                name: "FileType",
                schema: "FlexiFile");
        }
    }
}
