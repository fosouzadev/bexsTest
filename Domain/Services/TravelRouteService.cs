using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.DTO;

namespace Domain.Services
{
    public class TravelRouteService : ITravelRouteService
    {
        public IEnumerable<Route> GetRoutes() => CsvFileService.ReadFile();

        public string AddRoute(NewRoute newRoute) => CsvFileService.AddLineToFile(newRoute);
 
        public BestRoute CalculateBestRoute(string route, string destinationRoute)
        {
            var routes = GetRoutes();
            
            bool foundRoute = routes.Any(a => a.Name == route);
            bool foundDestinationRoute = routes.Any(a => a.DestinationRoutes.Select(s => s.Route.Name).Contains(destinationRoute));

            if (!foundRoute || !foundDestinationRoute)
                return new BestRoute { Error = "Routes not found" };

            int sum = 0;
            string destinationRouteAux = destinationRoute;
            Stack<Route> routesResult = new Stack<Route>();
            routesResult.Push(new Route(destinationRoute));

            do 
            {
                var routeAux = routes.Where(w => w.DestinationRoutes.Any(a => a.Route.Name == destinationRouteAux))
                                     .Select(s => new { Origin = s.Name, Value = s.DestinationRoutes.FirstOrDefault(f => f.Route.Name == destinationRouteAux)?.Value })
                                     .OrderBy(m => m.Value)
                                     .FirstOrDefault();

                if (routeAux != null)
                {
                    sum += routeAux.Value ?? 0;
                    destinationRouteAux = routeAux.Origin;
                    routesResult.Push(new Route(routeAux.Origin));
                }

                if (routeAux == null || destinationRouteAux == route)
                    destinationRouteAux = null;

            } while (destinationRouteAux != null);

            return new BestRoute
            {
                Value = sum,
                TravelRoutes = routesResult.Select(s => s.Name)
            };
        }
    }
}