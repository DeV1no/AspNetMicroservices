namespace Basket.Api.Entities;

public class BasketCheckout
{
    public string UserName { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }

    // BillingAddress
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string AddressLine { get; set; } = string.Empty;
    public string Country { get; set; }= string.Empty;
    public string State { get; set; }= string.Empty;
    public string ZipCode { get; set; }= string.Empty;

    // Payment
    public string CardName { get; set; }= string.Empty;
    public string CardNumber { get; set; }= string.Empty;
    public string Expiration { get; set; }= string.Empty;
    public string CVV { get; set; }= string.Empty;
    public int PaymentMethod { get; set; }
}