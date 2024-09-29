using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceProductManagement.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 9, 19, 3, 11, 14, 168, DateTimeKind.Local).AddTicks(909));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 7, 29, 3, 11, 14, 168, DateTimeKind.Local).AddTicks(922));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3,
                column: "OrderDate",
                value: new DateTime(2024, 6, 29, 3, 11, 14, 168, DateTimeKind.Local).AddTicks(925));

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerName", "OrderDate" },
                values: new object[,]
                {
                    { 4, "Last Month Jerald", new DateTime(2024, 8, 29, 3, 11, 14, 168, DateTimeKind.Local).AddTicks(927) },
                    { 5, "Last Month Kristian", new DateTime(2024, 9, 1, 3, 11, 14, 168, DateTimeKind.Local).AddTicks(928) },
                    { 6, "Last Month Samonte", new DateTime(2024, 9, 9, 3, 11, 14, 168, DateTimeKind.Local).AddTicks(929) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 9, 19, 3, 0, 38, 382, DateTimeKind.Local).AddTicks(449));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                column: "OrderDate",
                value: new DateTime(2024, 7, 29, 3, 0, 38, 382, DateTimeKind.Local).AddTicks(490));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3,
                column: "OrderDate",
                value: new DateTime(2024, 6, 29, 3, 0, 38, 382, DateTimeKind.Local).AddTicks(494));
        }
    }
}
