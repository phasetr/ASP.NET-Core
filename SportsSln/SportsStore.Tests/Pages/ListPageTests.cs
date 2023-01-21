using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;
using Xunit;

namespace SportsStore.Tests.Pages;

public class ListPageTests
{
    [Fact]
    public void Can_Use_Repository()
    {
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns(new[]
        {
            new Product {ProductID = 1, Name = "P1"},
            new Product {ProductID = 2, Name = "P2"}
        }.AsQueryable());
        var mockLogger = new Mock<ILogger<ListModel>>();

        var listModel = new ListModel(mock.Object, mockLogger.Object);
        listModel.OnGet(null);
        var prodArray = listModel.Products.ToArray();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("P1", prodArray[0].Name);
        Assert.Equal("P2", prodArray[1].Name);
    }

    [Fact]
    public void Can_Paginate()
    {
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns(new[]
        {
            new() {ProductID = 1, Name = "P1"},
            new Product {ProductID = 2, Name = "P2"},
            new Product {ProductID = 3, Name = "P3"},
            new Product {ProductID = 4, Name = "P4"},
            new Product {ProductID = 5, Name = "P5"}
        }.AsQueryable());
        var mockLogger = new Mock<ILogger<ListModel>>();
        var listModel = new ListModel(mock.Object, mockLogger.Object);

        listModel.OnGet(null, 2);

        var prodArray = listModel.Products.ToArray();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("P3", prodArray[0].Name);
        Assert.Equal("P4", prodArray[1].Name);
    }

    [Fact]
    public void Can_Send_Pagination_View_Model()
    {
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns(new[]
        {
            new() {ProductID = 1, Name = "P1"},
            new Product {ProductID = 2, Name = "P2"},
            new Product {ProductID = 3, Name = "P3"},
            new Product {ProductID = 4, Name = "P4"},
            new Product {ProductID = 5, Name = "P5"}
        }.AsQueryable());
        var mockLogger = new Mock<ILogger<ListModel>>();
        var listModel = new ListModel(mock.Object, mockLogger.Object);

        listModel.OnGet(null, 2);

        // Assert
        var pageInfo = listModel.PagingInfo;
        Assert.Equal(2, pageInfo.CurrentPage);
        Assert.Equal(2, pageInfo.ItemsPerPage);
        Assert.Equal(5, pageInfo.TotalItems);
        Assert.Equal(3, pageInfo.TotalPages);
    }

    [Fact]
    public void Can_Filter_Products()
    {
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns(new[]
        {
            new() {ProductID = 1, Name = "P1", Category = "Cat1"},
            new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
            new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
            new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
            new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
        }.AsQueryable());
        var mockLogger = new Mock<ILogger<ListModel>>();
        var listModel = new ListModel(mock.Object, mockLogger.Object);

        listModel.OnGet("Cat2");
        var result = listModel.Products.ToArray();

        Assert.Equal(2, result.Length);
        Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
        Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
    }

    [Fact]
    public void Generate_Category_Specific_Product_Count()
    {
        // Arrange
        var mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns(new[]
        {
            new() {ProductID = 1, Name = "P1", Category = "Cat1"},
            new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
            new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
            new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
            new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
        }.AsQueryable());
        var mockLogger = new Mock<ILogger<ListModel>>();
        var listModel = new ListModel(mock.Object, mockLogger.Object);

        int GetTotalItems(ListModel model, string? category)
        {
            model.OnGet(category);
            return model.PagingInfo.TotalItems;
        }

        // Action
        var res1 = GetTotalItems(listModel, "Cat1");
        var res2 = GetTotalItems(listModel, "Cat2");
        var res3 = GetTotalItems(listModel, "Cat3");
        var resAll = GetTotalItems(listModel, null);

        // Assert
        Assert.Equal(2, res1);
        Assert.Equal(2, res2);
        Assert.Equal(1, res3);
        Assert.Equal(5, resAll);
    }
}