using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backoffice.Migrations
{
    /// <inheritdoc />
    public partial class IsValidated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCanceled",
                table: "Booking",
                newName: "IsValidated");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsValidated",
                table: "Booking",
                newName: "IsCanceled");
        }
    }
}
