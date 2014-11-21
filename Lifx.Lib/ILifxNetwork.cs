using System;
using System.Collections.Generic;
using Lifx.Lib.Enums;

namespace Lifx.Lib
{
    public interface ILifxNetwork
    {
        event EventHandler BulbCollectionChanged;
        event EventHandler BulbGroupCollectionChanged;

        bool IsBulbSuccessfullyConnectedToWifi { get; }

        IEnumerable<IBulb> GetBulbs();
        IEnumerable<IBulbGroup> GetBulbGroups();
        IEnumerable<IAccessPoint> GetAccessPoints();
        IEnumerable<IGateway> GetGateways();

        void RegisterSender(Action<IGateway, byte[]> sender);

        Tuple<IGateway, IBulb, AnswerType> ReceivedPacket(string ipAddress, byte[] data);
    }
}