using Dapper;
using Discount.Api.Entities;
using Npgsql;

namespace Discount.Api.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;
    private NpgsqlConnection _connection;
    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connection =new NpgsqlConnection(_configuration
            .GetValue<string>("DatabaseSettings:ConnectionString"));;
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        await using var connection =
            new NpgsqlConnection(_configuration
                .GetValue<string>("DatabaseSettings:ConnectionString"));
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
            "Select * From Coupon Where ProductName=@ProductName"
            ,new{ProductName=productName});
        if (coupon == null)
            return new Coupon
            {
                Id = 0,
                ProductName = "Not Found 404",
                Description = "404",
                Amount = 0
            };
        return coupon;

    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        await using var connection =
            new NpgsqlConnection(_configuration
                .GetValue<string>("DatabaseSettings:ConnectionString"));
        var effected = await connection.ExecuteAsync
            (
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName,@Description, @Amount)",
                new {ProductName= coupon.ProductName, Description=coupon.Description, Amount= coupon.Amount }
            );
        return effected != 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        await using var connection =
            new NpgsqlConnection(_configuration
                .GetValue<string>("DatabaseSettings:ConnectionString"));
        var effected = await connection.ExecuteAsync
        (
            "UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id",
            new {ProductName= coupon.ProductName, Description=coupon.Description, Amount= coupon.Amount, Id=coupon.Id }
        );
        return effected != 0;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        await using var connection =
            new NpgsqlConnection(_configuration
                .GetValue<string>("DatabaseSettings:ConnectionString"));
        var effected = await connection.ExecuteAsync
        (
            "DELETE FROM  Coupon SET  WHERE ProductName=@ProductName",
            new {ProductName=productName }
        );
        return effected != 0;
    }
}