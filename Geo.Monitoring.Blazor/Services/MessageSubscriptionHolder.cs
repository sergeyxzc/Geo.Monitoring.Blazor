using System.Reactive.Disposables;

namespace Geo.Monitoring.Blazor.Services;

public class MessageSubscriptionHolder : IDisposable
{
    private readonly CompositeDisposable _compositeDisposable = new();

    public void AddSubscription(IDisposable subscription)
    {
        _compositeDisposable.Add(subscription);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}