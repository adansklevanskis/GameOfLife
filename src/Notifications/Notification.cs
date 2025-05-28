namespace GameOfLife.Notifications;

public class Notification
{
    public string Message { get; }
    public string Code { get; }

    public Notification(string message, string code = "")
    {
        Message = message;
        Code = code;
    }
}