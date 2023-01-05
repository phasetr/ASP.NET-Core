using System.Collections.Generic;
using Moq;
using SimpleApp.Controllers;
using SimpleApp.Models;
using Xunit;

namespace SimpleApp.Tests;

public class HomeControllerTests
{
    [Fact]
    public void IndexActionModelIsComplete()
    {
        // Arrange
        Product[] testData =
        {
            new() {Name = "P1", Price = 75.10M},
            new() {Name = "P2", Price = 120M},
            new() {Name = "P3", Price = 110M}
        };
        var mock = new Mock<IDataSource>();
        mock.SetupGet(m => m.Products).Returns(testData);
        var controller = new HomeController();
        controller.DataSource = mock.Object;

        // Act
        var model = controller.Index()?.ViewData.Model
            as IEnumerable<Product>;

        // Assert
        Assert.Equal(testData, model,
            Comparer.Get<Product>((p1, p2) => p1?.Name == p2?.Name
                                              && p1?.Price == p2?.Price));
        mock.VerifyGet(m => m.Products, Times.Once);
    }
}