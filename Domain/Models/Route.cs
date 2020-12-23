using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class Route
    {
        public Route(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public List<TravelRoute> DestinationRoutes { get; private set; } = new List<TravelRoute>();

        public void AddDestinationRoute(TravelRoute travelRoute)
        {
            if (travelRoute != null && !DestinationRoutes.Any(a => a.Route == travelRoute.Route))
                DestinationRoutes.Add(travelRoute);
        }
    }
}