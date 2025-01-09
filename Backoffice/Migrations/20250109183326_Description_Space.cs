using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backoffice.Migrations
{
    /// <inheritdoc />
    public partial class Description_Space : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpaceDescription",
                table: "Space",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<double>(
                name: "BookingPrice",
                table: "Booking",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpaceDescription",
                table: "Space");

            migrationBuilder.AlterColumn<double>(
                name: "BookingPrice",
                table: "Booking",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
