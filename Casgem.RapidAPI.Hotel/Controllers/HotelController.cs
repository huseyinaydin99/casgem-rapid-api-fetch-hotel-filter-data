using Casgem.RapidAPI.Hotel.Entities;
using Casgem.RapidAPI.Hotel.Models;
using Casgem.RapidAPI.Hotel.Services;
using Casgem.RapidAPI.Hotel.Utilities;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static Casgem.RapidAPI.Hotel.Entities.HotelEntity;

namespace Casgem.RapidAPI.Hotel.Controllers
{
    public class HotelController : Controller
    {
        private readonly HotelDataFetchUtil _dataFetchUtil;
        private readonly RedisService _redisService;
        static HotelDataModel hotelDataModel = new HotelDataModel();

        public HotelController(HotelDataFetchUtil dataFetchUtil, RedisService redisService)
        {
            _dataFetchUtil = dataFetchUtil;
            _redisService = redisService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HotelDataModel hotelDataModel = new HotelDataModel();
            var values = _dataFetchUtil.GetHotelDataWithRapidAPI();
            hotelDataModel.HotelDetailInfos = await values;

            _redisService.GetDb().StringSet("hotelDataModel", JsonConvert.SerializeObject(hotelDataModel.HotelDetailInfos));
            return View(hotelDataModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string search, string locale)
        {
            
            var values = _dataFetchUtil.GetHotelDataWithRapidAPISearch(search, locale);
            hotelDataModel.HotelDetailInfos = await values;
            _redisService.GetDb().StringSet("hotelDataModel", JsonConvert.SerializeObject(hotelDataModel.HotelDetailInfos));
            return View(hotelDataModel);
        }

        //BeforeFetchHotelInfoDataWriteExcelFile
        [HttpPost]
        [Route("/BeforeFetchHotelInfoDataWriteExcelFile")]
        public async Task<IActionResult> BeforeFetchHotelInfoDataWriteExcelFile(string fileName)
        {
            if (fileName != null)
            {
                // Excel dosyası oluşturma
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("RapidAPI otel arama cizelgesi.");
                    // Başlık satırı
                    worksheet.Cell("A1").Value = "HotelDetailInfoId";
                    worksheet.Cell("B1").Value = "geoId";
                    worksheet.Cell("C1").Value = "destinationId";
                    worksheet.Cell("D1").Value = "landmarkCityDestinationId";
                    worksheet.Cell("E1").Value = "type";
                    worksheet.Cell("F1").Value = "redirectPage";
                    worksheet.Cell("G1").Value = "latitude";
                    worksheet.Cell("H1").Value = "longitude";
                    worksheet.Cell("I1").Value = "searchDetail";
                    worksheet.Cell("J1").Value = "caption";
                    worksheet.Cell("K1").Value = "name";
                    // Veri satırları
                    int row = 2;
                    int IdCount = 0;
                    foreach (var entity in JsonConvert.DeserializeObject<List<HotelDetailInfo>>(_redisService.GetDb().StringGet("hotelDataModel")))
                    {
                        IdCount++;
                        worksheet.Cell($"A{row}").Value = IdCount.ToString();
                        worksheet.Cell($"B{row}").Value = entity.geoId;
                        worksheet.Cell($"C{row}").Value = entity.destinationId;
                        worksheet.Cell($"D{row}").Value = entity.landmarkCityDestinationId;
                        worksheet.Cell($"E{row}").Value = entity.type;
                        worksheet.Cell($"F{row}").Value = entity.redirectPage;
                        worksheet.Cell($"G{row}").Value = entity.latitude;
                        worksheet.Cell($"H{row}").Value = entity.longitude;

                        if (entity.searchDetail == null || entity.searchDetail == "" || entity.searchDetail == " ")
                            worksheet.Cell($"I{row}").Value = "Boş";
                        else
                            worksheet.Cell($"I{row}").Value = entity.searchDetail;

                        worksheet.Cell($"J{row}").Value = entity.caption;
                        worksheet.Cell($"K{row}").Value = entity.name;
                        row++;
                    }
                    // Excel dosyasını stream olarak kaydettim ve kullanıcıya indirme işlemi için son aşama
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
                    }
                }
            }
            else
            {
                HotelDataModel hotelDataModel = new HotelDataModel();
                var values = _dataFetchUtil.GetHotelDataWithRapidAPI();
                hotelDataModel.HotelDetailInfos = await values;
                //return View(hotelDataModel);
                return File(System.Text.UTF8Encoding.UTF8.GetBytes("selam"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
            }
        }

        [HttpPost]
        public async Task<IActionResult> WriteHotelInfoExcelFile(string search, string locale, string fileName)
        {
            if (search != null && locale != null && fileName != null)
            {
                var values = await _dataFetchUtil.GetHotelDataWithRapidAPISearch(search, locale);
                // Excel dosyası oluşturma
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Doviz Kuru Verileri");
                    // Başlık satırı
                    worksheet.Cell("A1").Value = "HotelDetailInfoId";
                    worksheet.Cell("B1").Value = "geoId";
                    worksheet.Cell("C1").Value = "destinationId";
                    worksheet.Cell("D1").Value = "landmarkCityDestinationId";
                    worksheet.Cell("E1").Value = "type";
                    worksheet.Cell("F1").Value = "redirectPage";
                    worksheet.Cell("G1").Value = "latitude";
                    worksheet.Cell("H1").Value = "longitude";
                    worksheet.Cell("I1").Value = "searchDetail";
                    worksheet.Cell("J1").Value = "caption";
                    worksheet.Cell("K1").Value = "name";
                    // Veri satırları
                    int row = 2;
                    int IdCount = 0;
                    foreach (var entity in values)
                    {
                        IdCount++;
                        worksheet.Cell($"A{row}").Value = IdCount.ToString();
                        worksheet.Cell($"B{row}").Value = entity.geoId;
                        worksheet.Cell($"C{row}").Value = entity.destinationId;
                        worksheet.Cell($"D{row}").Value = entity.landmarkCityDestinationId;
                        worksheet.Cell($"E{row}").Value = entity.type;
                        worksheet.Cell($"F{row}").Value = entity.redirectPage;
                        worksheet.Cell($"G{row}").Value = entity.latitude;
                        worksheet.Cell($"H{row}").Value = entity.longitude;

                        if(entity.searchDetail == null || entity.searchDetail == "" || entity.searchDetail == " ")
                            worksheet.Cell($"I{row}").Value = "Boş";
                        else
                        worksheet.Cell($"I{row}").Value = entity.searchDetail;

                        worksheet.Cell($"J{row}").Value = entity.caption;
                        worksheet.Cell($"K{row}").Value = entity.name;
                        row++;
                    }
                    // Excel dosyasını stream olarak kaydettim ve kullanıcıya indirme işlemi için son aşama
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
                    }
                }
            }
            else
            {
                HotelDataModel hotelDataModel = new HotelDataModel();
                var values = _dataFetchUtil.GetHotelDataWithRapidAPI();
                hotelDataModel.HotelDetailInfos = await values;
                return View(hotelDataModel);
            }
        }

        
    }

}

