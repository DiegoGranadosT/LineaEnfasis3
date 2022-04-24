using Microsoft.AspNetCore.Mvc;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Proyecto.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MQTTController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            string broker = "broker.emqx.io";
            int port = 1883;
            string topic = "incubus/message";
            string clientId = Guid.NewGuid().ToString();
            string username = "emqx";
            string password = "public";
            MqttClient client = ConnectMQTT(broker, port, clientId, username, password);
            Subscribe(client, topic);
            Publish(client, topic);
            return Ok();
        }

        public static MqttClient ConnectMQTT(string broker, int port, string clientId, string username, string password)
        {
            MqttClient client = new MqttClient(broker, port, false, MqttSslProtocols.None, null, null);
            client.Connect(clientId, username, password);
            if (client.IsConnected)
            {
                Console.WriteLine("Connected to MQTT Broker");
            }
            else
            {
                Console.WriteLine("Failed to connect");
            }
            return client;
        }

        public static void Publish(MqttClient client, string topic)
        {
            int msg_count = 0;
            System.Threading.Thread.Sleep(1 * 1000);
            string msg = "messages: " + msg_count.ToString();
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg));
            var aux = "Send `{ 0}` to topic `{ 1}`" + msg + topic;
            Console.WriteLine("Send `{0}` to topic `{1}`", msg, topic);
            msg_count++;

        }

        public static void Subscribe(MqttClient client, string topic)
        {
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        }
        public static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string payload = System.Text.Encoding.Default.GetString(e.Message);
            Console.WriteLine("Received `{0}` from `{1}` topic", payload, e.Topic.ToString());
        }

    }
}
