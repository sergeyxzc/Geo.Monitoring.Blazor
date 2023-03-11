namespace Geo.Monitoring.Blazor.Components.Common;

public abstract class BaseBusyApplicationComponent : BaseApplicationComponent
{
    public bool Busy { get; set; }

    private CancellationTokenSource _cancellationTokenSource;

    protected CancellationToken ComponentCancellationToken => (_cancellationTokenSource ??= new CancellationTokenSource()).Token;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await DoOnInitializedAsync();
        }
        catch
        {
            // ignored
        }
        finally
        {
            Busy = false;
        }
    }

    protected abstract Task DoOnInitializedAsync();
}