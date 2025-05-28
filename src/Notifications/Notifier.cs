namespace GameOfLife.Notifications;

public class Notifier
{
    private readonly List<Notification> _notifications = new();

    public void AddNotification(string message, string code = "")
    {
        _notifications.Add(new Notification(message, code));
    }

    public bool HasNotifications() => _notifications.Count > 0;

    public IEnumerable<Notification> GetNotifications() => _notifications;
}
