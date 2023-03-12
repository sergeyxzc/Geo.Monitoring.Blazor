using Geo.Monitoring.Blazor.Messages;
using Geo.Monitoring.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Geo.Monitoring.Blazor.Components.Common;

public abstract class BaseBusyApplicationComponent : BaseApplicationComponent
{
    [Inject] public IMessageService MessageService { get; set; }

    public bool Busy { get; set; }
    private readonly List<string> _busyCookie = new();

    private readonly MessageSubscriptionHolder _messageSubscriptionHolder = new();
    private CancellationTokenSource _cancellationTokenSource;

    protected CancellationToken ComponentCancellationToken => (_cancellationTokenSource ??= new CancellationTokenSource()).Token;

    protected override async Task OnInitializedAsync()
    {
        var busyCookie = Guid.NewGuid().ToString("D");
        try
        {
            var beginMessage = new BeginBusyMessage(this, busyCookie);
            BeginBusy(beginMessage.Cookie);
            MessageService.Subscribe<BeginBusyMessage>(_messageSubscriptionHolder, OnBeingBusyMessage);
            MessageService.Subscribe<EndBusyMessage>(_messageSubscriptionHolder, OnEndBusyMessage);

            MessageService.Publish(beginMessage);

            await DoOnInitializedAsync(_messageSubscriptionHolder);
        }
        catch
        {
            // ignored
        }
        finally
        {
            var endMessage = new EndBusyMessage(this, busyCookie);
            EndBusy(endMessage.Cookie);
            MessageService.Publish(endMessage);
        }
    }

    public override void Dispose()
    {
        _messageSubscriptionHolder.Dispose();
        base.Dispose();
    }

    protected virtual Task DoOnInitializedAsync(MessageSubscriptionHolder messageSubscriptionHolder)
    {
        return Task.CompletedTask;
    }

    private void OnBeingBusyMessage(BeginBusyMessage message)
    {
        if (message.Sender == this)
            return;
        BeginBusy(message.Cookie);
    }

    private void BeginBusy(string cookie)
    {
        if (_busyCookie.Contains(cookie))
            return;
        _busyCookie.Add(cookie);
        ChangeBusy(true);
    }

    private void OnEndBusyMessage(EndBusyMessage message)
    {
        if (message.Sender == this)
            return;
        EndBusy(message.Cookie);
    }

    private void EndBusy(string cookie)
    {
        _busyCookie.Remove(cookie);
        if (_busyCookie.Count == 0)
            ChangeBusy(false);
    }

    private void ChangeBusy(bool busy)
    {
        if (Busy == busy)
            return;
        Busy = busy;
        OnChangeBusy(Busy);
    }

    protected virtual void OnChangeBusy(bool busy)
    {
    }
}