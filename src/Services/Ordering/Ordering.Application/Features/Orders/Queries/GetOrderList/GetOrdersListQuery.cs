using System.Collections.Generic;
using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList;

public class GetOrdersListQuery:IRequest<List<OrdersVm>>
{

    public string UserName { get; set; } = string.Empty;

    public GetOrdersListQuery(string userName)
    {
        UserName=userName;
    }
}