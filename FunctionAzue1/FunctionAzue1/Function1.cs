using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunctionAzue1
{
    public class TempHumidityIoTTableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string DeviceId { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string DataMeasured { get; set; }

    }

    public static class Function1
    {
        [FunctionName("Function1")]
        [return: Table("DamasanjTable", Connection = "StorageConnectionAppSetting")]
        public static TempHumidityIoTTableEntity Run([IoTHubTrigger("messages/events", Connection = "ConnectionStringSetting")]EventData message,
                [ServiceBus("yourQueueOrTopicName", Connection = "ServiceBusConnectionSetting", EntityType = EntityType.Topic)]out string queueMessage,
                TraceWriter log)
        {
            var messageAsJson = Encoding.UTF8.GetString(message.GetBytes());
            log.Info($"C# IoT Hub trigger function processed a message: {messageAsJson}");

            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(messageAsJson);

            var deviceid = message.SystemProperties["iothub-connection-device-id"];
            queueMessage = null;
            if (data.ContainsKey("humidity"))
            {
                int humidity = int.Parse(data["humidity"]);

                if (humidity > 60)
                {
                    Dictionary<string, string> overHumidityThresholdMessage = new Dictionary<string, string>
                 {
                     { "deviceId",deviceid.ToString()},
                     { "humidity", humidity.ToString()},
                     {"message", "HighHumidityThreshold" }
                 };
                    queueMessage = JsonConvert.SerializeObject(overHumidityThresholdMessage);
                }
            }

            return new TempHumidityIoTTableEntity
            {
                PartitionKey = deviceid.ToString(),
                RowKey = $"{deviceid}{message.EnqueuedTimeUtc.Ticks}",
                DeviceId = deviceid.ToString(),
                Humidity = data.ContainsKey("humidity") ? data["humidity"] : "",
                Temperature = data.ContainsKey("temperature") ? data["temperature"] : "",
                DateMeasured = message.EnqueuedTimeUtc.ToString("O")
            };

        }

    }
}