using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeStore.Migrations
{
    /// <inheritdoc />
    public partial class updatesToDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CutomerId",
                table: "Carts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CutomerId",
                table: "Carts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
