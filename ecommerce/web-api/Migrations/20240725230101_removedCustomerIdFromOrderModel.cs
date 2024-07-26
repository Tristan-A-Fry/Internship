using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommapp.Migrations
{
    /// <inheritdoc />
    public partial class removedCustomerIdFromOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
