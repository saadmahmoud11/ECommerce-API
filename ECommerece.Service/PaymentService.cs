using AutoMapper;
using ECommerece.Domain.Contracts;
using ECommerece.Domain.Entities.OrderModule;
using ECommerece.Domain.Entities.ProductModule;
using ECommerece.ServiceAbstraction;
using ECommerece.Shared.BasketDTOS;
using ECommerece.Shared.CommonResult;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = ECommerece.Domain.Entities.ProductModule.Product;

namespace ECommerece.Service;

public class PaymentService : IPaymentService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork,
        IConfiguration configuration, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _mapper = mapper;
    }
    public async Task<Result<BasketDto>> CreatePaymentIntentAsync(string basketId)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket is null)
        {
            return Error.NotFound();
        }
        foreach (var item in basket.Items)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
            if (product is null)
            {
                return Error.NotFound();
            }
            item.Price = product.Price;
        }
        var subtotal = basket.Items.Sum(x => x.Quantity * x.Price);
        if (!basket.DeliveryMethodId.HasValue)
        {
            return Error.NotFound();
        }
        var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
        basket.ShippingCost = deliveryMethod!.Price;
        var amount = subtotal + deliveryMethod.Price;
        StripeConfiguration.ApiKey = _configuration["StripeOption:SecretKey"];
        PaymentIntentService paymentIntentService = new PaymentIntentService();
        PaymentIntent paymentIntent;
        if (basket.PaymentIntentId is null)
        {
            // create payment intent
            var option = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };
            paymentIntent = await paymentIntentService.CreateAsync(option);
        }
        else
        {
            // update payment intent
            var option = new PaymentIntentUpdateOptions
            {
                Amount = (long)(amount * 100)
            };
            paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntentId, option);
        }
        basket.PaymentIntentId = paymentIntent.Id;
        basket.ClientSecret = paymentIntent.ClientSecret;
        basket = await _basketRepository.CreateOrUpdateBasketAsync(basket);
        return _mapper.Map<BasketDto>(basket);
    }
}