using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_Management.Migrations
{
    public partial class orderModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DishId",
                table: "DishModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderIdId",
                table: "DishModels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DishModels_OrderIdId",
                table: "DishModels",
                column: "OrderIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishModels_OrderModels_OrderIdId",
                table: "DishModels",
                column: "OrderIdId",
                principalTable: "OrderModels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishModels_OrderModels_OrderIdId",
                table: "DishModels");

            migrationBuilder.DropIndex(
                name: "IX_DishModels_OrderIdId",
                table: "DishModels");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "DishModels");

            migrationBuilder.DropColumn(
                name: "OrderIdId",
                table: "DishModels");
        }
    }
}
