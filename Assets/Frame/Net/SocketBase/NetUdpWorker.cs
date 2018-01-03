using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public delegate void NetUdpDelegate(byte[] bytes, int length, string ip, ushort port);

public class NetUdpWorker {

    private NetUdpDelegate udpDelegate;

    private IPEndPoint udpPoint;

    private Socket udpSocket;

    private byte[] byteArr;

    private bool isReceive = true;
    public bool BindSocket(ushort port,int byteLength,NetUdpDelegate tmpDelegate) {
        udpPoint = new IPEndPoint(IPAddress.Any, port);
        UdpConnect();
        udpDelegate = tmpDelegate;
        byteArr = new byte[byteLength];
        Thread receiveThread = new Thread(ReceiveByte);
        receiveThread.Start();
        //使用if判断一些异常情况
        return true;
    }

    public void UdpConnect() {
        udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udpSocket.Bind(udpPoint);
    }

    void ReceiveByte() {
        while (isReceive) {
            if (udpSocket == null || udpSocket.Available < 1) {
                Thread.Sleep(100);
                continue;
            }
            lock (this) {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (EndPoint)sender;
                int count = udpSocket.ReceiveFrom(byteArr, ref remote);
                if (udpDelegate != null) {
                    udpDelegate(byteArr, count, remote.AddressFamily.ToString(), (ushort)sender.Port);
                }
            }
        }
    }


    public int SendUpdData(string desIp, byte[] data, ushort port) {
        IPEndPoint desPoint = new IPEndPoint(IPAddress.Parse(desIp), port);
        if (!udpSocket.Connected) {
            UdpConnect();
        }
        int sendCount = udpSocket.SendTo(data, data.Length, SocketFlags.None, desPoint);
        return sendCount;
    }

}
