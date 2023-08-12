using Casgem.RapidAPI.Hotel.Models;
using Casgem.RapidAPI.Hotel.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Casgem.RapidAPI.Hotel.Controllers
{
    public class HotelController : Controller
    {
        public async Task<IActionResult> Index()
        {
            HotelDataModel hotelDataModel = new HotelDataModel();
            using(var util = new HotelDataFetchUtil())
            {
                var values = util.GetHotelDataWithRapidAPI();
                hotelDataModel.HotelDetailInfos = await values;
            }
            return View(hotelDataModel);
        }
    }
}
