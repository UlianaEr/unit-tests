using Moq;
using Orders.Abstractions;
using Orders.Handlers;
using Orders.Models;

namespace Orders.Tests;

public class OrderCreatedHandlerTests
{
    readonly Mock<INotificationSender> _sender = new();
    readonly OrderCreatedHandler _sut;

    public OrderCreatedHandlerTests() => _sut = new(_sender.Object);

    [Fact]
    public async Task HandleAsync_ValidMessage_SendsNotification()
    {
        var id = Guid.NewGuid();
        var message = new OrderCreated(id);
        await _sut.HandleAsync(message);
        _sender.Verify(s => s.SendAsync(It.Is<string>(t => t.Contains(id.ToString()))), Times.Once());
    }

    [Fact]
    public async Task HandleAsync_EmptyOrderId_Throws()
    {
        var message = new OrderCreated(Guid.Empty);
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.HandleAsync(message));
        _sender.Verify(s => s.SendAsync(It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public async Task HandleAsync_NullMessage_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.HandleAsync(null!));
    }
}