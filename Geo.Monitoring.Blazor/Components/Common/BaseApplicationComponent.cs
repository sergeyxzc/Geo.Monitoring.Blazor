using Microsoft.AspNetCore.Components;

namespace Geo.Monitoring.Blazor.Components.Common;

public abstract class BaseApplicationComponent : ComponentBase, IDisposable
{
    private CancellationTokenSource _cancellationTokenSource;

    protected CancellationToken ComponentCancellationToken => (_cancellationTokenSource ??= new CancellationTokenSource()).Token;

    public virtual void Dispose()
    {
        if (_cancellationTokenSource == null)
            return;
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }
}