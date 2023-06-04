using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
