using ApiRest.Controllers;
using Domain.Interfaces;
using Domain.Models.DTO;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Tests.ApiRestTests
{
    public class TravelRouteControllerTest
    {
        private readonly TravelRouteController _travelRouteController;

        public TravelRouteControllerTest()
        {
            ITravelRouteService travelRouteService = new TravelRouteService();
            _travelRouteController = new TravelRouteController(travelRouteService);
        }

        [Fact]
        public void AddRoute_AddValidRoute_ReturnOkOrBadRequestResult()
        {
            NewRoute newRoute = new NewRoute { Route = "ABC", DestinationRoute = "XYZ", Value = 35 };

            IActionResult response = _travelRouteController.AddRoute(newRoute);

            if (response is OkResult)
                Assert.IsType<OkResult>(response);
            else
            {
                BadRequestObjectResult badRequestResult = response as BadRequestObjectResult;
                string error = Assert.IsType<string>(badRequestResult.Value);

                Assert.Equal("Route already registered", error);
            }
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
        public void AddRoute_AddInvalidRoute_ReturnBadRequestResult(string route, string destinationRoute, int value)
        {
            NewRoute newRoute = new NewRoute { Route = route, DestinationRoute = destinationRoute, Value = value };

            IActionResult response = _travelRouteController.AddRoute(newRoute);
            BadRequestObjectResult badRequestResult = response as BadRequestObjectResult;
            
            string error = Assert.IsType<string>(badRequestResult.Value);

            Assert.Equal("Inválid values", error);
        }

        [Theory]
        [InlineData("GRU", "CDG")]
        [InlineData("BRC", "CDG")]
        public void CalculateBestRoute_ChecksTheRouteAtTheLowestCost_ReturnOkResult(string from, string to)
        {
            IActionResult response = _travelRouteController.CalculateBestRoute(from, to);
            OkObjectResult okResult = response as OkObjectResult;

            var bestRoute = Assert.IsType<BestRoute>(okResult.Value);

            Assert.True(string.IsNullOrEmpty(bestRoute.Error));
            Assert.NotEmpty(bestRoute.TravelRoutes);
            Assert.True(bestRoute.Value > 0);
        }

        [Fact]
        public void CalculateBestRoute_SendInvalidRoutes_ReturnBadRequestResult()
        {
            string from = "GRU";
            string to = "BBB";

            IActionResult response = _travelRouteController.CalculateBestRoute(from, to);
            BadRequestObjectResult badRequestResult = response as BadRequestObjectResult;

            var bestRoute = Assert.IsType<BestRoute>(badRequestResult.Value);

            Assert.False(string.IsNullOrEmpty(bestRoute.Error));
            Assert.Equal("Routes not found", bestRoute.Error);
        }
    }
}