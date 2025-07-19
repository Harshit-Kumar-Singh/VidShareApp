namespace VidShareWebApi.Services.KafkaService
{
    public interface IKafkaService
    {
        Task SendMessageAsync(string topic, string message);
    }
}