using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backoffice.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerFirstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    EquipmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentDesignation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EquipmentAvailable = table.Column<int>(type: "int", nullable: false),
                    EquipmentInStock = table.Column<int>(type: "int", nullable: false),
                    EquipmentPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.EquipmentID);
                });

            migrationBuilder.CreateTable(
                name: "Space",
                columns: table => new
                {
                    SpaceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpaceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpaceCapacity = table.Column<int>(type: "int", nullable: false),
                    SpacePrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Space", x => x.SpaceID);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    SpaceID = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingPaidAmount = table.Column<double>(type: "float", nullable: true),
                    BookingPrice = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_Booking_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Space_SpaceID",
                        column: x => x.SpaceID,
                        principalTable: "Space",
                        principalColumn: "SpaceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingEquip",
                columns: table => new
                {
                    BookingEquipID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    EquipmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingEquip", x => x.BookingEquipID);
                    table.ForeignKey(
                        name: "FK_BookingEquip_Booking_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Booking",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingEquip_Equipment_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipment",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CustomerID",
                table: "Booking",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_SpaceID",
                table: "Booking",
                column: "SpaceID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingEquip_BookingID",
                table: "BookingEquip",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingEquip_EquipmentID",
                table: "BookingEquip",
                column: "EquipmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingEquip");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Space");
        }
    }
}
