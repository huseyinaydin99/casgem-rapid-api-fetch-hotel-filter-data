using AutoMapper;
using Casgem.RapidAPI.Hotel.DAL.DapperConfiguration.Abstract;
using Casgem.RapidAPI.Hotel.Entities;
using Casgem.RapidAPI.Hotel.Utilities.Constants;
using Dapper;
using Newtonsoft.Json;
using System.Data;

namespace Casgem.RapidAPI.Hotel.Utilities
{
    public class HotelDataFetchUtil : IDisposable
    {
        private readonly IMapper _mapper;
        private readonly IDapper _dapper;

        public HotelDataFetchUtil()
        {
        }

        public HotelDataFetchUtil(IMapper mapper, IDapper dapper)
        {
            _mapper = mapper;
            _dapper = dapper;
        }

        public async Task<List<HotelDetailInfo>> GetHotelDataWithRapidAPI()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(URLConstant.RAPID_API_URL),
                Headers =
                {
                    { "X-RapidAPI-Key", "dd3fa5539bmsh3b20c335e1895adp104c66jsn47d0556c5e5e" },
                    { "X-RapidAPI-Host", "hotels4.p.rapidapi.com" },
                },
            };
            var response = await HttpClientConstant.CLIENT.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            List<HotelEntity> values = JsonConvert.DeserializeObject<List<HotelEntity>>(body);
            List<HotelDetailInfo> hotelDetailInfos = await ConvertToJson(values);
            SaveHotelDetailToDatabase(hotelDetailInfos);
            return hotelDetailInfos;
        }

        public async Task<List<HotelDetailInfo>> ConvertToJson(List<HotelEntity> values)
        {
            List<HotelDetailInfo> hotelDetailInfos = new List<HotelDetailInfo>();
            values.ForEach(v =>
            {
                foreach (var item in v.suggestions)
                {
                    foreach (var entity in item.entities)
                    {
                        var value = _mapper.Map<HotelDetailInfo>(entity);
                        hotelDetailInfos.Add(value);
                    }
                }
            });
            
            return hotelDetailInfos;
        }

        public async Task SaveHotelDetailToDatabase(List<HotelDetailInfo> hotelDetailInfos)
        {
            hotelDetailInfos.ForEach(hotelInfo =>
            {
                string sql = "INSERT INTO HOTEL_INFO(destinationId, landmarkCityDestinationId, type, redirectPage, latitude, longitude, searchDetail, caption, name) VALUES(@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@p1", hotelInfo.destinationId, DbType.String);
                parameters.Add("@p2", hotelInfo.landmarkCityDestinationId, DbType.String);
                parameters.Add("@p3", hotelInfo.type, DbType.String);
                parameters.Add("@p4", hotelInfo.redirectPage, DbType.String);
                parameters.Add("@p5", hotelInfo.latitude, DbType.Double);
                parameters.Add("@p6", hotelInfo.longitude, DbType.Double);
                parameters.Add("@p7", hotelInfo.searchDetail, DbType.String);
                parameters.Add("@p8", hotelInfo.caption, DbType.String);
                parameters.Add("@p9", hotelInfo.name_, DbType.String);
                var result = Task.FromResult(_dapper.Insert<int>("[dbo].[SP_Add_Hotel]", parameters, commandType: CommandType.StoredProcedure));
            });
        }

        public void Dispose()
        {
            
        }
    }
}
