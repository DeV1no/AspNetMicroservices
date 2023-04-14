using System.Net;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class BasketController
{
    private readonly IBasketRepository _repository;
    private readonly DiscountGrpcService _discountGrpcService;
    public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService)
    {
        _repository = repository;
        _discountGrpcService = discountGrpcService;
    }
    [HttpGet("{userName}",Name="GetBasket")]
    [ProducesResponseType(typeof(IEnumerable<ShoppingCart>), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        => (await _repository.GetBasket(userName))??new ShoppingCart(userName);

    [HttpPut]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        // todo : connect to Discount Grpc  and calculate latest price 

        foreach (var item in basket.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Mount;
        }
        return await _repository.UpdateBasket(basket);
    }

    [HttpDelete("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task DeleteBasket(string userName)
    {
        await _repository.DeleteBasket(userName);
        
    }

}