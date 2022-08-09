using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_Management.Migrations
{
    public partial class orderModel3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "DishIdId",
                table: "OrderModels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "orderId",
                table: "OrderModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderModels_DishIdId",
                table: "OrderModels",
                column: "DishIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderModels_DishModels_DishIdId",
                table: "OrderModels",
                column: "DishIdId",
                principalTable: "DishModels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderModels_DishModels_DishIdId",
                table: "OrderModels");

            migrationBuilder.DropIndex(
                name: "IX_OrderModels_DishIdId",
                table: "OrderModels");

            migrationBuilder.DropColumn(
                name: "DishIdId",
                table: "OrderModels");

            migrationBuilder.DropColumn(
                name: "orderId",
                table: "OrderModels");

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
    }
}
