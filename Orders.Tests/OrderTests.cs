using Orders.Models;

namespace Orders.Tests;

public class OrderTests
{
    [Fact]
    public void Total_TwoItems_ReturnsSum()
    {
        var order = new Order();
        order.Add("Книга", 300m, 2);
        order.Add("Ручка", 50m, 1);
        var total = order.Total();
        Assert.Equal(650m, total);
    }

    [Fact]
    public void Total_EmptyOrder_ReturnsZero()
    {
        var order = new Order();
        var total = order.Total();
        Assert.Equal(0m, total);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Add_InvalidName_Throws(string? name)
    {
        var order = new Order();
        Assert.Throws<ArgumentException>(() => order.Add(name!, 100m, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Add_NonPositivePrice_Throws(decimal price)
    {
        var order = new Order();
        Assert.Throws<ArgumentOutOfRangeException>(() => order.Add("Книга", price, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void Add_InvalidQuantity_Throws(int qty)
    {
        var order = new Order();
        Assert.Throws<ArgumentOutOfRangeException>(() => order.Add("Книга", 100m, qty));
    }

    [Fact]
    public void Add_ValidItem_AddsToItems()
    {
        var order = new Order();
        order.Add("Книга", 300m, 2);
        var item = Assert.Single(order.Items);
        Assert.Equal("Книга", item.Name);
        Assert.Equal(300m, item.Price);
        Assert.Equal(2, item.Quantity);
    }

    [Fact]
    public void Constructor_WithItems_SameTotalAsAdd()
    {
        var items = new List<OrderItem>
        {
            new("Книга", 300m, 2),
            new("Ручка", 50m, 1),
        };
        var order = new Order(items);
        var manual = new Order();
        manual.Add("Книга", 300m, 2);
        manual.Add("Ручка", 50m, 1);
        Assert.Equal(manual.Total(), order.Total());
        Assert.Equal(650m, order.Total());
    }
}