using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository( OrderContext orderContext)
        : base(orderContext)
    {
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
    
       => await _orderContext.Orders
            .Where(x => x.UserName == userName).ToListAsync();
    
}