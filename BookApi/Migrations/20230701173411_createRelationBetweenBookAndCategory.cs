using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookApi.Migrations
{
    /// <inheritdoc />
    public partial class createRelationBetweenBookAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a9887e1-d486-455e-ba30-2a5fea781d20");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "25b3e5e4-2275-43d0-bbe5-0c6affce1cc8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "623205a6-7ca9-4b61-b87c-3b2e2aad226b");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "120b4701-4497-46bc-8ed8-ca9ed51ea158", "6c41359e-e09c-463c-aec8-3cbff3f34f22", "User", "USER" },
                    { "14782299-02bf-409e-ad9b-4050a9f0d6ac", "76c0645d-bbd8-4fe2-8d11-d00568e8b648", "Admin", "ADMIN" },
                    { "9f858193-8af0-43ea-863a-142916074461", "60686665-7b04-4d28-a998-a082d81532b3", "Editor", "EDITOR" }
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 1, "Körlük" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 1, "Jack London" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 2, "Suç ve Ceza" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Categories_CategoryId",
                table: "Books",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Categories_CategoryId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_CategoryId",
                table: "Books");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "120b4701-4497-46bc-8ed8-ca9ed51ea158");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14782299-02bf-409e-ad9b-4050a9f0d6ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f858193-8af0-43ea-863a-142916074461");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Books");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a9887e1-d486-455e-ba30-2a5fea781d20", "fd15b518-b12e-4b96-ba52-edf10b56da72", "Admin", "ADMIN" },
                    { "25b3e5e4-2275-43d0-bbe5-0c6affce1cc8", "7c82bd3e-3d90-48d4-8b98-941db4b93731", "Editor", "EDITOR" },
                    { "623205a6-7ca9-4b61-b87c-3b2e2aad226b", "76b7d31e-32d6-4471-ac72-852f6dc79190", "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Kitap 1");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Kitap 2");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Kitap 3");
        }
    }
}
