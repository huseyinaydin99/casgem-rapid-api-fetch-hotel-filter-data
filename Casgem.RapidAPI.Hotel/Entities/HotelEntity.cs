namespace Casgem.RapidAPI.Hotel.Entities
{
    public class HotelEntity
    {
        public string term { get; set; }
        public int moresuggestions { get; set; }
        public object autoSuggestInstance { get; set; }
        public string trackingID { get; set; }
        public bool misspellingfallback { get; set; }
        public Suggestion[] suggestions { get; set; }
        

        public class Suggestion
        {
            public string group { get; set; }
            public Entity[] entities { get; set; }
        }

        public class Entity
        {
            public string geoId { get; set; }
            public string destinationId { get; set; }
            public string landmarkCityDestinationId { get; set; }
            public string type { get; set; }
            public string redirectPage { get; set; }
            public float latitude { get; set; }
            public float longitude { get; set; }
            public object searchDetail { get; set; }
            public string caption { get; set; }
            public string name { get; set; }
        }
        public bool geocodeFallback { get; set; }

    }
}
