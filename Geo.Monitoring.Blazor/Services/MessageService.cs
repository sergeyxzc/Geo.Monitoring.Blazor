using System.Reactive.Linq;
using System.Reactive.Subjects;
using static DevExpress.Blazor.Internal.Rx;

namespace Geo.Monitoring.Blazor.Services;

public interface IMessageService
{
    void Publish<TMessage>(TMessage message);

    IObservable<TMessage> Messages<TMessage>();
}

public class MessageService : IMessageService, IDisposable
{
    private readonly Subject<object> _subject = new();

    public void Dispose()
    {
        _subject.Dispose();
    }

    public void Publish<TMessage>(TMessage message)
    {
        _subject.OnNext(message);
    }

    public IObservable<TMessage> Messages<TMessage>()
    {
        return _subject.OfType<TMessage>().AsObservable();
    }
}
