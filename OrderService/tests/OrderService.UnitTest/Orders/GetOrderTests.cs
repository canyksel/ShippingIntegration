using AutoMapper;
using Moq;
using OrderService.Application.Orders.Queries.GetOrderByOrderNumber;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories.Order;

namespace OrderService.Unit.Test.Orders;

public class GetOrderTests
{
    [Fact]
    public async Task HappyPath()
    {
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();

        var mockOrder = new Mock<Order>();
        var expectedDto = new GetOrderByOrderNumberDto { OrderNumber = mockOrder.Object.OrderNumber };

        mockOrderRepository.Setup(r => r.GetByOrderNumberWithDetailsAsync(mockOrder.Object.OrderNumber))
                           .ReturnsAsync(mockOrder.Object);

        mockMapper.Setup(m => m.Map<GetOrderByOrderNumberDto>(mockOrder.Object))
                  .Returns(expectedDto);

        var handler = new GetOrderByOrderNumberQueryHandler(mockOrderRepository.Object, mockMapper.Object);

        var result = await handler.Handle(new GetOrderByOrderNumberQuery { OrderNumber = mockOrder.Object.OrderNumber }, default);

        result.Equals(expectedDto);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenOrderNotFound()
    {
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockMapper = new Mock<IMapper>();

        mockOrderRepository.Setup(r => r.GetByOrderNumberWithDetailsAsync(It.IsAny<string>()))
                           .ReturnsAsync((Order)null!);

        var handler = new GetOrderByOrderNumberQueryHandler(mockOrderRepository.Object, mockMapper.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new GetOrderByOrderNumberQuery { OrderNumber = "ORD-999" }, default));
    }
}
