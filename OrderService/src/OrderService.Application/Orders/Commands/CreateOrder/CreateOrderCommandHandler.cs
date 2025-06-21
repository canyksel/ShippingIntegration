using MediatR;
using OrderService.Application.Extensions;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories.Order;
using OrderService.Domain.Repositories.Product;
using OrderService.Domain.Repositories.ShippingCompany;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IShippingCompanyRepository shippingCompanyRepository,
    IProductRepository productRepository) : IRequestHandler<CreateOrderCommand, CreateOrderResultDto>
{
    public async Task<CreateOrderResultDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var shippingCompany = await shippingCompanyRepository.GetByIdAsync(request.ShippingCompanyId) ??
            throw new InvalidOperationException("Shipping company not found.");

        var orderAddress = new OrderAddress(request.Address.Country, request.Address.City, request.Address.State,
                 request.Address.PostalCode, request.Address.AddressTitle, request.Address.AddressDetail);

        var order = new Order(request.SellerName, request.BuyerName, orderAddress, request.PaymentType, shippingCompany, new List<OrderProduct>());
        orderAddress.SetOrderIformation(order);

        var orderProducts = new List<OrderProduct>();
        foreach (var item in request.Products)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new InvalidOperationException($"Product not found: {item.ProductId}");

            if (product.Stock < item.Quantity)
                throw new InvalidOperationException($"Not enough stock for product: {product.Name}");

            product.DecreaseStock(item.Quantity);

            var orderProduct = new OrderProduct(order, product, item.UnitPrice, item.Quantity);
            order.AddProduct(orderProduct);
        }

        foreach (var item in orderProducts)
        {
            item.SetOrder(order);
        }

        await orderRepository.AddAsync(order);
        await orderRepository.UnitOfWork.SaveEntitesAsync(cancellationToken);

        return new CreateOrderResultDto
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            OrderStatus = order.Status.GetDescription(),
            PaymentType = order.PaymentType.GetDescription(),
        };
    }
}