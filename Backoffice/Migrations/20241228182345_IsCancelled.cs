using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backoffice.Migrations
{
    /// <inheritdoc />
    public partial class IsCancelled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "Booking",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Booking",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Booking");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");
        }
    }
}
