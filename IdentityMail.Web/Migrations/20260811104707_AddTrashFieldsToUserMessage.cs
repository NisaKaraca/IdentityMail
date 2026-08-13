using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityMail.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddTrashFieldsToUserMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPermanentlyDeletedByReceiver",
                table: "UserMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPermanentlyDeletedBySender",
                table: "UserMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrashedByReceiver",
                table: "UserMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrashedBySender",
                table: "UserMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPermanentlyDeletedByReceiver",
                table: "UserMessages");

            migrationBuilder.DropColumn(
                name: "IsPermanentlyDeletedBySender",
                table: "UserMessages");

            migrationBuilder.DropColumn(
                name: "IsTrashedByReceiver",
                table: "UserMessages");

            migrationBuilder.DropColumn(
                name: "IsTrashedBySender",
                table: "UserMessages");
        }
    }
}
