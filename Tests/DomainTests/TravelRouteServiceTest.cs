using Domain.Interfaces;
using Domain.Models.DTO;
using Domain.Services;
using Xunit;

namespace Tests.DomainTests
{
    public class TravelRouteServiceTest
    {
        private readonly ITravelRouteService _travelRouteService;

        public TravelRouteServiceTest()
        {
            _travelRouteService = new TravelRouteService();
        }

        [Fact]
        public void GetRoutes_ReadRoutesFromCsvFile_ReturnRoutes()
        {
            var routes = _travelRouteService.GetRoutes();

            Assert.NotEmpty(routes);
        }

        [Fact]
        public void AddRoute_AddValidRoute_NewRouteAvailable()
        {
            NewRoute newRoute = new NewRoute { Route = "ABC", DestinationRoute = "XYZ", Value = 35 };

            string error = _travelRouteService.AddRoute(newRoute);

            Assert.True(error == null || error == "Route already registered");
        }

        [Theory]
        [InlineData("", "ABC", 45)]
        [InlineData(null, "ABC", 45)]
        [InlineData("ABC", "", 45)]
        [InlineData("ABC", null, 45)]
        [InlineData("XYZ", "ABC", 0)]
        [InlineData("", "", 0)]
        [InlineData(null, null, 0)]
        [InlineData("A", "B", 3)]
        [InlineData("A", "BC", 3)]
        [InlineData("AA", "B", 3)]
        public void AddRoute_AddInvalidRoute_ReturnErrorMessage(string route, string destinationRoute, int value)
        {
            NewRoute newRoute = new NewRoute { Route = route, DestinationRoute = destinationRoute, Value = value };

            string error = _travelRouteService.AddRoute(newRoute);

            Assert.Equal("Inválid values", error);
        }

        [Theory]
        [InlineData("GRU", "CDG", "GRU - BRC - SCL - ORL - CDG > $40")]
        [InlineData("BRC", "CDG", "BRC - SCL - ORL - CDG > $30")]
        public void CalculateBestRoute_ChecksTheRouteAtTheLowestCost_ReturnBestRouteAndCost(string route, string destinationRoute, string expected)
        {
            var bestRoute = _travelRouteService.CalculateBestRoute(route, destinationRoute);
            string actual = $"{string.Join(" - ", bestRoute.TravelRoutes)} > ${bestRoute.Value}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CalculateBestRoute_SendInvalidRoutes_ReturnErrorMessage()
        {
            string route = "GRU";
            string destinationRoute = "BBB";

            var bestRoute = _travelRouteService.CalculateBestRoute(route, destinationRoute);

            Assert.Equal("Routes not found", bestRoute.Error);
        }
    }
}