using AutoMapper;
using ECommerece.Domain.Contracts;
using ECommerece.Domain.Entities.BasketModule;
using ECommerece.Domain.Entities.OrderModule;
using ECommerece.Domain.Entities.ProductModule;
using ECommerece.ServiceAbstraction;
using ECommerece.Shared.CommonResult;
using ECommerece.Shared.OrderDTOS;

namespace ECommerece.Service;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IMapper mapper, IBasketRepository basketRepository,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email)
    {
        var orderAddress = _mapper.Map<OrderAddress>(orderDto.Address);
        var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId);
        if (basket is null)
        {
            return Error.NotFound("Basket not found", $"Basket with id {orderDto.BasketId} not found");
        }
        List<OrderItem> orderItems = [];
        foreach (var item in basket.Items)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
            if (product is null)
            {
                return Error.NotFound("Product not found", $"Product with id {item.Id} not found");
            }
            orderItems.Add(CreateOrderItem(item, product));
        }
        var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);
        if (deliveryMethod is null)
        {
            return Error.NotFound("Delivery method not found", $"Delivery method with id {orderDto.DeliveryMethodId} not found");
        }
        var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
        var order = new Order()
        {
            UserEmail = email,
            OrderAddress = orderAddress,
            OrderItems = orderItems,
            DeliveryMethod = deliveryMethod,
            Subtotal = subtotal
        };
        await _unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
        int result = await _unitOfWork.SaveChangeAsync();
        if (result <= 0)
        {
            return Error.Failure("Order creation failed", "An error occurred while creating the order");
        }
        return _mapper.Map<OrderToReturnDto>(order);
    }

    private static OrderItem CreateOrderItem(BasketItem item, Product product)
    {
        return new OrderItem()
        {
            Product = new ProductItemOrder()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                PictureUrl = product.PictureUrl
            },
            Price = product.Price,
            Quantity = item.Quantity
        };
    }
}
