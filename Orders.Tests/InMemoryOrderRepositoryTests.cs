using Orders.Models;
using Orders.Repositories;

namespace Orders.Tests;

public class InMemoryOrderRepositoryTests
{
    [Fact]
    public async Task SaveAsync_ThenFindAsync_ReturnsSameOrder()
    {
        var repo = new InMemoryOrderRepository();
        var order = new Order();
        order.Add("Книга", 300m, 1);
        await repo.SaveAsync(order);
        var found = await repo.FindAsync(order.Id);
        Assert.NotNull(found);
        Assert.Equal(order.Id, found!.Id);
        Assert.Equal(order.Total(), found.Total());
    }

    [Fact]
    public async Task FindAsync_UnknownId_ReturnsNull()
    {
        var repo = new InMemoryOrderRepository();
        var found = await repo.FindAsync(Guid.NewGuid());
        Assert.Null(found);
    }

    [Fact]
    public async Task SaveAsync_Null_Throws()
    {
        var repo = new InMemoryOrderRepository();
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.SaveAsync(null!));
    }
}