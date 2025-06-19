using FluentValidation;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.SellerName)
            .NotEmpty().WithMessage("Seller name is required.");

        RuleFor(x => x.BuyerName)
            .NotEmpty().WithMessage("Buyer name is required.");

        RuleFor(x => x.ShippingCompanyId)
            .NotEmpty().WithMessage("Shipping company is required.");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("At least one product must be added.");

        RuleForEach(x => x.Products).SetValidator(new OrderProductDtoValidator());
        RuleFor(x => x.Address).SetValidator(new OrderAddressDtoValidator());
    }
}

public class OrderProductDtoValidator : AbstractValidator<OrderProductDto>
{
    public OrderProductDtoValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThan(0);
    }
}

public class OrderAddressDtoValidator : AbstractValidator<OrderAddressDto>
{
    public OrderAddressDtoValidator()
    {
        RuleFor(x => x.Country).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.PostalCode).NotEmpty();
        RuleFor(x => x.AddressTitle).NotEmpty();
        RuleFor(x => x.AddressDetail).NotEmpty();
    }
}