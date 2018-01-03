using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TcpEvent {
    TcpConnect = ManagerID.NetManager + 1,
    TcpSend ,
    MaxValue,
}

public class TcpConnectMsg :MsgBase{

    public string ip;
    public ushort port;
    public TcpConnectMsg(ushort _msgId,string _ip,ushort _port){
        this.MsgID = _msgId;
        ip = _ip;
        port = _port;
    }
}
//???   继承NetMsgBase
public class TcpSendMsg : MsgBase
{
    public NetMsgBase netMsg;
    public TcpSendMsg(ushort _msgId, NetMsgBase _netMsg) {
        netMsg = _netMsg;
        this.MsgID = _msgId;
    }
}
public class NetTcp :NetBase {

    private NetTcpWorker NetTcpWorker = null;
    void Awake() {
        this.msgIds = new ushort[] {
            (ushort)TcpEvent.TcpConnect,
            (ushort)TcpEvent.TcpSend,
        };
        RegisterSelfMsg(this, msgIds);
    }



    public override void HandleMsgEvent(MsgBase msg)
    {
        switch (msg.MsgID) {
            case (ushort)TcpEvent.TcpConnect:
                TcpConnectMsg tcpMsg = (TcpConnectMsg)msg;
                NetTcpWorker = new NetTcpWorker(tcpMsg.ip, tcpMsg.port);
                break;
            case (ushort)TcpEvent.TcpSend:
                TcpSendMsg sendMsg = (TcpSendMsg)msg;
                if (NetTcpWorker != null) NetTcpWorker.PushSendMsg(sendMsg.netMsg);
                break;
        }  
    }

    void Update() {
        if (NetTcpWorker != null) {
            NetTcpWorker.Update();
        }
    }

   
}
