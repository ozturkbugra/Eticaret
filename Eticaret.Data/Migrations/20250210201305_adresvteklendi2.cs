using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eticaret.Data.Migrations
{
    /// <inheritdoc />
    public partial class adresvteklendi2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 2, 10, 23, 13, 4, 170, DateTimeKind.Local).AddTicks(9778), new Guid("f26d174e-d0e5-43c0-986e-f255fdc9e0d3") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 2, 10, 23, 13, 4, 175, DateTimeKind.Local).AddTicks(522));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 2, 10, 23, 13, 4, 175, DateTimeKind.Local).AddTicks(2238));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 2, 10, 23, 7, 33, 821, DateTimeKind.Local).AddTicks(3147), new Guid("c394f328-9a3a-4a46-b68b-5a1983c7188e") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 2, 10, 23, 7, 33, 825, DateTimeKind.Local).AddTicks(4057));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 2, 10, 23, 7, 33, 825, DateTimeKind.Local).AddTicks(6070));
        }
    }
}
