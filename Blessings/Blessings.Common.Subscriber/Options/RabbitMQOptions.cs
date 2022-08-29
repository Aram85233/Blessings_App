namespace Blessings.Common.Subscriber.Options
{
    public class RabbitMQOptions
    {
        public const string Section = "RabbitMQSubscriber";
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public string DeadLetterExchange { get; set; }
        public string DeadLetterQueue { get; set; }
    }
}
