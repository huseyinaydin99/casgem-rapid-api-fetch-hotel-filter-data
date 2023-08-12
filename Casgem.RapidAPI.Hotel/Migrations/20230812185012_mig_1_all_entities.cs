using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casgem.RapidAPI.Hotel.Migrations
{
    public partial class mig_1_all_entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotelDetailInfos",
                columns: table => new
                {
                    HotelDetailInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    geoId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    destinationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    landmarkCityDestinationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    redirectPage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    latitude = table.Column<float>(type: "real", nullable: false),
                    longitude = table.Column<float>(type: "real", nullable: false),
                    searchDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelDetailInfos", x => x.HotelDetailInfoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelDetailInfos");
        }
    }
}
