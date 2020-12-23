using System.Collections.Generic;

namespace Domain.Models.DTO
{
    public class BestRoute
    {
        public IEnumerable<string> TravelRoutes { get; set; }
        public int Value { get; set; }
        public string Error { get; set; }
    }
}