using MediatR;
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

        var address = new OrderAddress(
           request.Address.Country,
           request.Address.City,
           request.Address.State,
           request.Address.PostalCode,
           request.Address.AddressTitle,
           request.Address.AddressDetail
       );

        var orderProducts = new List<OrderProduct>();
        foreach (var item in request.Products)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new InvalidOperationException($"Product not found: {item.ProductId}");

            if (product.Stock < item.Quantity)
                throw new InvalidOperationException($"Not enough stock for product: {product.Name}");

            product.DecreaseStock(item.Quantity); // stok düş
            var orderProduct = new OrderProduct(null!, product, item.UnitPrice, item.Quantity);
            orderProducts.Add(orderProduct);
        }

        Order order = new(request.SellerName, request.BuyerName, address, request.PaymentType, shippingCompany, orderProducts);

        foreach (var item in orderProducts)
        {
            item.SetOrder(order);
        }

        await orderRepository.AddAsync(order);
        await productRepository.UnitOfWork.SaveEntitesAsync(cancellationToken);

        return new CreateOrderResultDto
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber
        };
    }
}