using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casgem.RapidAPI.Hotel.Migrations
{
    public partial class mig_1_all_entities_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotelDetailInfos",
                columns: table => new
                {
                    HotelDetailInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    geoId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    destinationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    landmarkCityDestinationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    redirectPage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latitude = table.Column<float>(type: "real", nullable: true),
                    longitude = table.Column<float>(type: "real", nullable: true),
                    searchDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
