using Casgem.RapidAPI.Hotel.Entities;
using Microsoft.EntityFrameworkCore;

namespace Casgem.RapidAPI.Hotel.DAL
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options) 
        {

        }
        public DbSet<HotelDetailInfo> HotelDetailInfos { get; set; }
    }
}
