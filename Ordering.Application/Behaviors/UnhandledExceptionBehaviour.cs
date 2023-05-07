using MediatR;

namespace Ordering.Application.Behaviors;

public class UnhandledExceptionBehaviour<TRequest,TResponse>:IPipelineBehavior<TRequest,TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}