using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eticaret.Data.Migrations
{
    /// <inheritdoc />
    public partial class siparişdurumueklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderState",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 2, 11, 23, 9, 41, 850, DateTimeKind.Local).AddTicks(914), new Guid("74755e12-8580-411e-94d0-9c74258e8bc1") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 2, 11, 23, 9, 41, 854, DateTimeKind.Local).AddTicks(1117));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 2, 11, 23, 9, 41, 854, DateTimeKind.Local).AddTicks(2898));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppUserID",
                table: "Orders",
                column: "AppUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AppUsers_AppUserID",
                table: "Orders",
                column: "AppUserID",
                principalTable: "AppUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AppUsers_AppUserID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AppUserID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderState",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 2, 11, 21, 41, 23, 743, DateTimeKind.Local).AddTicks(6735), new Guid("7b037764-63de-410f-8cdf-61859ef9a42b") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 2, 11, 21, 41, 23, 747, DateTimeKind.Local).AddTicks(7692));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 2, 11, 21, 41, 23, 747, DateTimeKind.Local).AddTicks(9378));
        }
    }
}
