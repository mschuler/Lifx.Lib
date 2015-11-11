Lifx.Lib
========

Portable Class Library (PCL) to control LIFX Bulbs

Installation
------------

    PM> Install-Package Lifx.Lib

Usage
-------

### Create DatagramSocket and connect it with the library

    private const string Port = "56700";
    private static readonly ILifxNetwork _network = LifxNetworkFactory.Instance;
    private static readonly DatagramSocket _socket;

    static LifxNetworkService()
    {
        _socket = new DatagramSocket();
        _socket.Control.DontFragment = true;
        _socket.MessageReceived += HandleIncomingMessages;

        var connectionProfile = NetworkInformation.GetConnectionProfiles().FirstOrDefault(
            p => p.IsWlanConnectionProfile && p.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.None);

        if (connectionProfile != null)
        {
            _socket.BindServiceNameAsync(Port, connectionProfile.NetworkAdapter);
        }

        _network.RegisterSender(SendCommand);
    }

    private static async void SendCommand(IGateway gateway, byte[] data)
    {
        using (var stream = await _socket.GetOutputStreamAsync(new HostName(IpProvider.BroadcastAddress), Port))
        {
            await stream.WriteAsync(data.AsBuffer(0, data.Length));
        }
    }

    private static void HandleIncomingMessages(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs e)
    {
        var address = e.RemoteAddress.ToString();

        using (var reader = e.GetDataReader())
        {
            var data = reader.DetachBuffer().ToArray();
            _network.ReceivedPacket(address, data);
        }

    }

### Scan Network

    var gateways = _network.GetGateways();
    foreach (var gateway in gateways)
    {
        // search for bulbs in already known gateways
        _network.ScanGateway(gateway);
    }

    // search for new gateways
    _network.ScanNetwork(IpProvider.BroadcastAddress);

### Perform bulb actions

    _network.ChangeColor(bulb, hsvColor, 0);
	
	_network.SwitchOn(bulb);
	_network.SwitchOff(bulb);

# License

Developed by [Michael Schuler](https://www.michaelschuler.ch) under the [MIT License](LICENSE)
