using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaAlertasBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class ArregloRerlacionLecturas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LecturaEntitys_SensorEntitys_SensorEntidadSensorId",
                table: "LecturaEntitys");

            migrationBuilder.DropIndex(
                name: "IX_LecturaEntitys_SensorEntidadSensorId",
                table: "LecturaEntitys");

            migrationBuilder.DropColumn(
                name: "SensorEntidadSensorId",
                table: "LecturaEntitys");

            migrationBuilder.CreateIndex(
                name: "IX_LecturaEntitys_SensorId",
                table: "LecturaEntitys",
                column: "SensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_LecturaEntitys_SensorEntitys_SensorId",
                table: "LecturaEntitys",
                column: "SensorId",
                principalTable: "SensorEntitys",
                principalColumn: "SensorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LecturaEntitys_SensorEntitys_SensorId",
                table: "LecturaEntitys");

            migrationBuilder.DropIndex(
                name: "IX_LecturaEntitys_SensorId",
                table: "LecturaEntitys");

            migrationBuilder.AddColumn<int>(
                name: "SensorEntidadSensorId",
                table: "LecturaEntitys",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LecturaEntitys_SensorEntidadSensorId",
                table: "LecturaEntitys",
                column: "SensorEntidadSensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_LecturaEntitys_SensorEntitys_SensorEntidadSensorId",
                table: "LecturaEntitys",
                column: "SensorEntidadSensorId",
                principalTable: "SensorEntitys",
                principalColumn: "SensorId");
        }
    }
}
