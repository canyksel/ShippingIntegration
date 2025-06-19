using AutoMapper;
using OrderService.Application.Orders.Queries.GetOrderByOrderNumber;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders.Maps;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, GetOrderByOrderNumberDto>()
          .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.ShippingCompanyName, opt => opt.MapFrom(src => src.ShippingCompany.Name))
          .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType.ToString()))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
          .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
          .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        CreateMap<OrderAddress, AddressDto>()
            .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.AddressSummary));

        CreateMap<OrderProduct, ProductDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name));
    }
}