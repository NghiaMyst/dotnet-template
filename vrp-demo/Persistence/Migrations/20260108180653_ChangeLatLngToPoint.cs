using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace vrp_demo.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLatLngToPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lat",
                table: "driver");

            migrationBuilder.DropColumn(
                name: "lng",
                table: "driver");

            migrationBuilder.AddColumn<Point>(
                name: "location",
                table: "driver",
                type: "geography (point, 4326)",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location",
                table: "driver");

            migrationBuilder.AddColumn<double>(
                name: "lat",
                table: "driver",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "lng",
                table: "driver",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
