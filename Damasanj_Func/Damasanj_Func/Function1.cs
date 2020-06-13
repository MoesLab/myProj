using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Damasanj_Func
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

    /*
    public static class Function1
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("Function1")]
        [return: Table("DamasanjTable", Connection = "StorageConAppSet")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "ConStrSetName")]EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
        }
    } */

    public static class Function1
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("Function1")]
        [return: Table("DamasanjTable", Connection = "StorageConAppSet")]
        public static TempHumidityIoTTableEntity Run([IoTHubTrigger("messages/events", Connection = "ConnectionStringSetting")]EventData message, ILogger log)
        {
            var messageAsJson = Encoding.UTF8.GetString(message.GetBytes());
            log.Info($"C# IoT Hub trigger function processed a message: {messageAsJson}");

            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(messageAsJson);

            var deviceid = message.SystemProperties["iothub-connection-device-id"];

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
