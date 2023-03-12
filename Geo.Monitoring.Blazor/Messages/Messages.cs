namespace Geo.Monitoring.Blazor.Messages
{
    public record BeginBusyMessage(object Sender, string Cookie);
    public record EndBusyMessage(object Sender, string Cookie);
}
