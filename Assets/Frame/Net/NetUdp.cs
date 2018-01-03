using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UdpEvent {
    Initial = TcpEvent.MaxValue + 1,
    SendData,
    MaxValue,
}
public class UdpInitMsg : MsgBase {
    public ushort port;
    public int recvLength;
    public NetUdpDelegate recvDelegate;
    public UdpInitMsg(ushort _port, int _recvLength, NetUdpDelegate _recvDelegate) {
        port = _port;
        recvLength = _recvLength;
        recvDelegate = _recvDelegate;
    }
}

public class UdpSendMsg : MsgBase {
    public string desIp;
    public byte[] data;
    public ushort port;
    public UdpSendMsg(ushort _port, string _desIp, byte[] _data) {
        port = _port;
        desIp = _desIp;
        data = _data;
    }
}
public class NetUdp : NetBase {
    private NetUdpWorker udpWorker;

    public override void HandleMsgEvent(MsgBase msg)
    {
        switch (msg.MsgID) {
            case (ushort)UdpEvent.Initial:
                udpWorker = new NetUdpWorker();
                UdpInitMsg udpMsg = (UdpInitMsg)msg;
                udpWorker.BindSocket(udpMsg.port, udpMsg.recvLength, udpMsg.recvDelegate);

                break;
            case (ushort)UdpEvent.SendData:
                UdpSendMsg sendMsg = (UdpSendMsg)msg;
                if (udpWorker != null) udpWorker.SendUpdData(sendMsg.desIp, sendMsg.data, sendMsg.port);
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Awake () {
        this.msgIds = new ushort[] {
            (ushort)UdpEvent.Initial,
            (ushort)UdpEvent.SendData,
        };
        RegisterSelfMsg(this, msgIds);
	}
	
	
}
