using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Lifx.Lib;
using Lifx.Lib.Enums;

namespace Lifx.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            LifxNetworkService.StartLifxNetwork();

            do
            {
                Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
            } while (!LifxNetworkFactory.Instance.GetBulbs().Any());


            //ConnectToWlan();
            //Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(3)));

            SetColor();
            Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(3)));

            SwitchOff();
            Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(3)));

            SwitchOn();
            Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(3)));

            DimOff();
            Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(5)));

            PrintBulbDetail();

            Console.ReadLine();
        }

        private static void PrintBulbDetail()
        {
            var network = LifxNetworkFactory.Instance;

            foreach (var bulb in network.GetBulbs())
            {
                network.ReadBulbInfo(bulb);
            }

            var waitTask = Task.Delay(TimeSpan.FromSeconds(3));
            Task.WaitAll(waitTask);

            foreach (var bulb in network.GetBulbs())
            {
                var vp = bulb.Version.Product;
                var vv = bulb.Version.Vendor;
                var ve = bulb.Version.Version;

                Console.WriteLine(
                    "Mac='{0}' Name='{1}' Color='{2}' PowerState={3} Version={4}/{5}/{6}",
                    bulb.MacString, bulb.Name, bulb.Color, bulb.IsPowerOn, vp, vv, ve);
            }
        }

        private static void DimOff()
        {
            var network = LifxNetworkFactory.Instance;

            foreach (var bulb in network.GetBulbs())
            {
                Console.WriteLine("Switch bulb '{0}' off", bulb.MacString);
                network.ChangeColor(bulb, new HsvColor(210, 0.5, 0.0), 2.0);
            }
        }

        private static void SwitchOff()
        {
            var network = LifxNetworkFactory.Instance;

            foreach (var bulb in network.GetBulbs())
            {
                Console.WriteLine("Switch bulb '{0}' off", bulb.MacString);
                network.SwitchOff(bulb);
            }
        }

        private static void SwitchOn()
        {
            var network = LifxNetworkFactory.Instance;

            foreach (var bulb in network.GetBulbs())
            {
                Console.WriteLine("Switch bulb '{0}' on", bulb.MacString);
                network.SwitchOn(bulb);
            }
        }

        private static void SetColor()
        {
            var network = LifxNetworkFactory.Instance;

            foreach (var bulb in network.GetBulbs())
            {
                if (!bulb.IsPowerOn)
                {
                    network.SwitchOn(bulb);
                }

                Console.WriteLine("Dim bulb '{0}' to 2%", bulb.MacString);
                network.ChangeColor(bulb, new HsvColor(210, 0.55, 0.02, 2500), 0);
            }
        }

        private static void ConnectToWlan()
        {
            var network = LifxNetworkFactory.Instance;

            var accessPoint = network.GetAccessPoints().FirstOrDefault(ap => ap.Ssid.StartsWith("FRITZ!Box", StringComparison.OrdinalIgnoreCase));
            if (accessPoint != null)
            {
                Console.WriteLine("Access point '{0}' found.", accessPoint.Ssid);
                var gateways = network.GetGateways();
                foreach (var gateway in gateways)
                {
                    network.SetAccessPoint(gateway, accessPoint, "...");
                }
            }
        }
    }

    public static class LifxNetworkService
    {
        private static readonly ILifxNetwork _network = LifxNetworkFactory.Instance;

        public static event EventHandler<Tuple<IGateway, IBulb, AnswerType>> ReceivedPacket;

        public static void StartLifxNetwork()
        {
            Task.Run(() => Receive());

            _network.RegisterSender(SendCommand);

            var hostName = Dns.GetHostName();
            var hostEntry = Dns.GetHostEntry(hostName);
            var ipAddresses = hostEntry.AddressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork).ToList();

            foreach (var ipAddress in ipAddresses)
            {
                var parts = ipAddress.ToString().Split(new[] { '.' }).Take(3);
                var address = string.Join(".", parts) + ".255";

                Task.Run(async () =>
                {
                    while (true)
                    {
                        _network.ScanNetwork(address);
                        _network.ScanAccessPoints(address);
                        var seconds = _network.GetBulbs().Any() ? 600 : 3;
                        await Task.Delay(TimeSpan.FromSeconds(seconds));
                    }
                });
            }
        }

        private static void SendCommand(IGateway gateway, byte[] data)
        {
            var udpClient = new UdpClient(gateway.IpAddress, 56700);
            udpClient.Send(data, data.Length);
        }

        private static async void Receive()
        {
            var udpClient = new UdpClient(56700);

            while (true)
            {
                var result = await udpClient.ReceiveAsync();
                if (result.Buffer.Length > 0)
                {
                    if (result.RemoteEndPoint.Port == 56700)
                    {
                        var packet = _network.ReceivedPacket(result.RemoteEndPoint.Address.ToString(), result.Buffer);
                        OnReceivedPacket(packet);
                    }
                }
            }
        }

        private static void OnReceivedPacket(Tuple<IGateway, IBulb, AnswerType> e)
        {
            var handler = ReceivedPacket;
            if (handler != null)
            {
                handler(null, e);
            }
        }
    }
}
