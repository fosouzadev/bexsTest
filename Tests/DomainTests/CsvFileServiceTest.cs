using Domain.Models.DTO;
using Domain.Services;
using Xunit;

namespace Tests.DomainTests
{
    public class CsvFileServiceTest
    {
        [Fact]
        public void ReadFile_ReadCsvFile_ReturnRoutes()
        {
            var routes = CsvFileService.ReadFile();

            Assert.NotEmpty(routes);
        }

        [Fact]
        public void AddLineToFile_AddValidRoute_NewRouteAvailable()
        {
            NewRoute newRoute = new NewRoute { Route = "ABC", DestinationRoute = "XYZ", Value = 35 };

            string error = CsvFileService.AddLineToFile(newRoute);

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
        public void AddLineToFile_AddInvalidRoute_ReturnErrorMessage(string route, string destinationRoute, int value)
        {
            NewRoute newRoute = new NewRoute { Route = route, DestinationRoute = destinationRoute, Value = value };

            string error = CsvFileService.AddLineToFile(newRoute);

            Assert.Equal("Inválid values", error);
        }
    }
}