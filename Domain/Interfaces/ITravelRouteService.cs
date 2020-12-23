using System.Collections.Generic;
using Domain.Models;
using Domain.Models.DTO;

namespace Domain.Interfaces
{
    public interface ITravelRouteService
    {
        IEnumerable<Route> GetRoutes();
        string AddRoute(NewRoute newRoute);
        BestRoute CalculateBestRoute(string route, string destinationRoute);
    }
}