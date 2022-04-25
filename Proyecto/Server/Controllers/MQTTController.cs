using Microsoft.AspNetCore.Mvc;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Proyecto.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MQTTController : ControllerBase
    {
        public readonly string broker = "broker.emqx.io";
        public readonly int port = 1883;
        public readonly string clientId = Guid.NewGuid().ToString();
        public readonly string username = "emqx";
        public readonly string password = "public";

        [HttpGet("temperatura")]
        public async Task<ActionResult> GetTemperatura1()
        {
            string topic = "incubusget/temperatura_1";

            MqttClient client = ConnectMQTT(broker, port, clientId, username, password);
            Subscribe(client, topic);
            return Ok();
        }

        [HttpGet("humedad")]
        public async Task<ActionResult> GetOrden()
        {
            string topic = "incubusget/humedad_1";

            MqttClient client = ConnectMQTT(broker, port, clientId, username, password);
            Subscribe(client, topic);
            return Ok();
        }

        [HttpGet("estado/agua")]
        public async Task<ActionResult> GetEstadoAgua()
        {
            string topic = "incubusget/estado_1";

            MqttClient client = ConnectMQTT(broker, port, clientId, username, password);
            Subscribe(client, topic);
            return Ok();
        }

        [HttpGet("estado/ventilador")]
        public async Task<ActionResult> GetEstadoVentilador()
        {
            string topic = "incubusget/estado_2";

            MqttClient client = ConnectMQTT(broker, port, clientId, username, password);
            Subscribe(client, topic);
            return Ok();
        }

        [HttpGet("estado/nivelAgua")]
        public async Task<ActionResult> GetEstadonivelAgua()
        {
            string topic = "incubusget/estado_3";

            MqttClient client = ConnectMQTT(broker, port, clientId, username, password);

            Subscribe(client, topic);
            return Ok();
        }

        [HttpPut("ventilador/{estado}")]
        public async Task<ActionResult> PutVentilador(bool estado)
        {
            string topic = "incubuspush/ventilador";

            MqttClient client = ConnectMQTT(broker, port, clientId, username, password);
            var a = estado.GetHashCode().ToString();
            return Ok(client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(estado.GetHashCode().ToString())));
        }

        [HttpPut("bomba/{estado}")]
        public async Task<ActionResult> PutBomba(bool estado) 
        {
            string topic = "incubuspush/bomba";

            MqttClient client = ConnectMQTT(broker, port, clientId, username, password);

            return Ok(client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(estado.GetHashCode().ToString())));
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

        public void Publish(MqttClient client, string topic)
        {
            int msg_count = 25;
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
