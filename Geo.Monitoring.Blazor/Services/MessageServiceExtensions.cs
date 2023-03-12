namespace Geo.Monitoring.Blazor.Services;

public static class MessageServiceExtensions
{
    public static void Subscribe<TMessage>(this IMessageService service, MessageSubscriptionHolder holder, Action<TMessage> handler)
    {
        holder.AddSubscription(service.Messages<TMessage>().Subscribe(handler));
    }

    public static IDisposable Subscribe<TMessage>(this IMessageService service, Action<TMessage> handler)
    {
        return service.Messages<TMessage>().Subscribe(handler);
    }
}