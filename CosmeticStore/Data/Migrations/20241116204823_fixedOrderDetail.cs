using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixedOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OrderDetail",
                newName: "OrderDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "OrderDetail",
                newName: "Id");
        }
    }
}
