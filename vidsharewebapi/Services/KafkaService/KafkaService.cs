using System.Formats.Asn1;
using Confluent.Kafka;

namespace VidShareWebApi.Services.KafkaService
{
    public class KafkaProducerService : IKafkaService
    {
        private readonly string bootstrapServer = "localhost:9092";
        public async Task SendMessageAsync(string topic, string message)
        {

            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServer
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                var deliveryResult = await producer.ProduceAsync("test-topic", new Message<Null, string> { Value = message });
                Console.WriteLine($"Message sent to {deliveryResult.TopicPartitionOffset}");

            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"Error sending message : {ex.Error.Reason}");
            }
        }



    }
}