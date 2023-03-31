using System.Net;
using Discount.Api.Entities;
using Discount.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _repository;

    public DiscountController(IDiscountRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{productName}", Name = "GetDiscount")]
    [ProducesResponseType(typeof(Coupon),(int)HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        => Ok(await _repository.GetDiscount(productName));

    [HttpPost]
    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        => Ok(await _repository.CreateDiscount(coupon));
    [HttpPut]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> UpdateDiscount(Coupon coupon)
    => Ok(await _repository.UpdateDiscount(coupon));

    [HttpDelete]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        => Ok(await _repository.DeleteDiscount(productName));

}