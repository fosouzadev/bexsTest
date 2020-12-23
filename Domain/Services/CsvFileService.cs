using System.IO;
using System.Collections.Generic;
using Domain.Models;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using Domain.Models.DTO;

[assembly: InternalsVisibleTo("Tests")]
namespace Domain.Services
{
    internal static class CsvFileService
    {
        private static string GetPath() => AppDomain.CurrentDomain.BaseDirectory;

        public static IEnumerable<Route> ReadFile()
        {
            List<Route> routes = new List<Route>();

            using (StreamReader stream = new StreamReader($@"{GetPath()}\input-file.csv"))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] data = line.Split(",");
                    string routeName = data[0];
                    string destinationRouteName = data[1];
                    int.TryParse(data[2], out int value);

                    Route route = routes.FirstOrDefault(f => f.Name == routeName);
                    Route destinationRoute = routes.FirstOrDefault(f => f.Name == destinationRouteName);

                    if (route == null)
                    {
                        route = new Route(routeName);
                        routes.Add(route);
                    }

                    if (destinationRoute == null)
                        destinationRoute = new Route(destinationRouteName);
                        
                    route.AddDestinationRoute(new TravelRoute(destinationRoute, value));
                }

                stream.Close();
            }

            return routes;
        }

        public static string AddLineToFile(NewRoute newRoute)
        {
            if (string.IsNullOrEmpty(newRoute.Route) ||
                string.IsNullOrEmpty(newRoute.DestinationRoute) ||
                newRoute.Value == 0 ||
                newRoute.Route.Length < 3 ||
                newRoute.DestinationRoute.Length < 3)
                return "Inválid values";

            newRoute.Route = newRoute.Route.ToUpper().Substring(0, 3);
            newRoute.DestinationRoute = newRoute.DestinationRoute.ToUpper().Substring(0, 3);

            var routes = ReadFile();

            if (!routes.Any(a => a.Name == newRoute.Route && 
                                 a.DestinationRoutes.Any(b => b.Route.Name == newRoute.DestinationRoute &&
                                                              b.Value == newRoute.Value)))
            {
                using (StreamWriter stream = new StreamWriter($@"{GetPath()}\input-file.csv", true))
                {
                    stream.WriteLine($"{newRoute.Route},{newRoute.DestinationRoute},{newRoute.Value}");
                    stream.Close();
                    return null;
                }
            }
            
            return "Route already registered";
        }
    }
}