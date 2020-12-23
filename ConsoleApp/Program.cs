using System;
using Domain.Interfaces;
using Domain.Models.DTO;
using Domain.Services;

namespace ConsoleApp
{
    public class Program
    {
        private static readonly ITravelRouteService _travelRouteService = new TravelRouteService();

        public static void Main(string[] args)
        {
            Console.Write("please enter the route (example GRU-CDG): ");
            string travelRoutes = Console.ReadLine();

            if (!TravelRoutesAreValid(travelRoutes))
            {
                Console.WriteLine("Invalid routes");
                return;
            }

            string[] data = travelRoutes.Split('-');
            string route = data[0];
            string destinationRoute = data[1];

            BestRoute bestRoute = _travelRouteService.CalculateBestRoute(route, destinationRoute);

            if (!string.IsNullOrEmpty(bestRoute.Error))
            {
                Console.WriteLine(bestRoute.Error);
                return;
            }

            Console.WriteLine($"best route: {string.Join(" - ", bestRoute.TravelRoutes)} > ${bestRoute.Value}");
        }

        private static bool TravelRoutesAreValid(string travelRoutes) =>
            !string.IsNullOrEmpty(travelRoutes) &&
            travelRoutes.Length == 7 &&
            travelRoutes.Contains("-");
    }
}