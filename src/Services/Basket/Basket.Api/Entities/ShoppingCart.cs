namespace Basket.Api.Entities;

public class ShoppingCart
{
    public ShoppingCart()
    {

    }

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

    public string UserName { get; set; } = string.Empty;
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

    public decimal TotalPrice
    {
        get
        {
            return Items.Sum(item => item.Price * item.Quantity);
        }
    }
}