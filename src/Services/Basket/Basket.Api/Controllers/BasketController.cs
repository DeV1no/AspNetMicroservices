using System.Net;
using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class BasketController
{
    private readonly IBasketRepository _repository;

    public BasketController(IBasketRepository repository)
    {
        _repository = repository;
    }
    [HttpGet("{userName}",Name="GetBasket")]
    [ProducesResponseType(typeof(IEnumerable<ShoppingCart>), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        => (await _repository.GetBasket(userName))??new ShoppingCart(userName);

    [HttpPut]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart Basket)
    {
        return await _repository.UpdateBasket(Basket);
    }

    [HttpDelete("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task DeleteBasket(string userName)
    {
        await _repository.DeleteBasket(userName);
        
    }

}