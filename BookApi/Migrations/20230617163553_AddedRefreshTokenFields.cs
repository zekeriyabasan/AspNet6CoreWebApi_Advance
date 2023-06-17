using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedRefreshTokenFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "30eab8ee-69a3-4409-9223-f49df3835765");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64a34867-4858-4105-bfad-de2d483ee63d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "728aaab3-c483-4edb-ab59-6e6e2de36957");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpireTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14cbc899-f833-46e0-b430-b70f1e8a8468", "511fb31a-caad-4a50-9573-bfb119ea163d", "User", "USER" },
                    { "a0a337a0-5df5-4190-bfde-73b35fd6fbce", "2f0dc74c-de76-43df-85f5-1db987598e5d", "Editor", "EDITOR" },
                    { "a2eeef5b-0557-4b11-9366-574298c7f037", "42dc7784-23b3-4da6-8d91-c1bbcd5b6062", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14cbc899-f833-46e0-b430-b70f1e8a8468");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0a337a0-5df5-4190-bfde-73b35fd6fbce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2eeef5b-0557-4b11-9366-574298c7f037");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpireTime",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "30eab8ee-69a3-4409-9223-f49df3835765", "1ac0cf29-6e11-4787-91c5-be4dd0579191", "Editor", "EDITOR" },
                    { "64a34867-4858-4105-bfad-de2d483ee63d", "616fd8fe-d168-45e5-800e-7df949e77907", "User", "USER" },
                    { "728aaab3-c483-4edb-ab59-6e6e2de36957", "0451117e-953e-4891-a904-8106a8719dea", "Admin", "ADMIN" }
                });
        }
    }
}
