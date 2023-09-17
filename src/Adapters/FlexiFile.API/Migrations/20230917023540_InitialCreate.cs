using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

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
                    mime_types = table.Column<string[]>(type: "character varying[]", nullable: false)
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
                    approved_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
                    approved_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_update_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
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
                    to_type_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    handler_class_name = table.Column<string>(type: "character varying", nullable: false)
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
                    submitted_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    finished_upload = table.Column<bool>(type: "boolean", nullable: false),
                    finished_upload_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true)
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
                    last_update_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: true),
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
                    timestamp = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false)
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
                    created_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false)
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
                    file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_type_conversion_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, comment: "(\"InQueue\",\"InProgress\",\"Completed\",\"Failed\")"),
                    percentage_complete = table.Column<double>(type: "double precision", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    last_update_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    extra_info = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FileConversion_pk", x => x.id);
                    table.ForeignKey(
                        name: "FileConversion_File_id_fk",
                        column: x => x.file_id,
                        principalSchema: "FlexiFile",
                        principalTable: "File",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_FileConversion_FileTypeConversion",
                        column: x => x.file_type_conversion_id,
                        principalSchema: "FlexiFile",
                        principalTable: "FileTypeConversion",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FileConversionResult",
                schema: "FlexiFile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_conversion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
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
                name: "IX_FileConversion_file_id",
                schema: "FlexiFile",
                table: "FileConversion",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileConversion_file_type_conversion_id",
                schema: "FlexiFile",
                table: "FileConversion",
                column: "file_type_conversion_id");

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
                name: "FileConversion",
                schema: "FlexiFile");

            migrationBuilder.DropTable(
                name: "File",
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
