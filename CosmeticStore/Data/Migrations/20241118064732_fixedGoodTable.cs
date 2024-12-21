using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixedGoodTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Good",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Good_UserId",
                table: "Good",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Good_AspNetUsers_UserId",
                table: "Good",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Good_AspNetUsers_UserId",
                table: "Good");

            migrationBuilder.DropIndex(
                name: "IX_Good_UserId",
                table: "Good");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Good");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "AspNetUsers");
        }
    }
}
