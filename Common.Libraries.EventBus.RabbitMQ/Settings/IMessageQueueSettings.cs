namespace Common.Libraries.EventBus.RabbitMQ.Settings
{
    public interface IMessageQueueSettings
    {
        string HostName { get; set; }
        string Password { get; set; }
        string Port { get; set; }
        string Username { get; set; }
       string Exchange { get; set; }
    }
}