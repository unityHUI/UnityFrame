using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
public class NetTcpWorker
{
    private Queue<NetMsgBase> recvQueue = null;
    private Queue<NetMsgBase> sendQueue = null;
    private NetSocket clientSocket;
    private Thread sendThread;

    public NetTcpWorker(string ip, ushort port)
    {
        recvQueue = new Queue<NetMsgBase>();
        sendQueue = new Queue<NetMsgBase>();
        clientSocket = new NetSocket();
        clientSocket.AsynConnect(ip, port, ConnectCallBack, ReceiveCallBack);
    }

    private void ConnectCallBack(bool isSuccess, SocketError errorType, string exception)
    {
        if (isSuccess)
        {
            sendThread = new Thread(LoopSendMsg);
            sendThread.Start();
        }
    }

    private void ReceiveCallBack(bool isSuccess, SocketError errorType, string exception, byte[] msgBytes, string str)
    {
        if (isSuccess)
        {
            NetMsgBase msg = new NetMsgBase(msgBytes);
            PushRecvMsg(msg);
        }
        else
        {//处理接收消息发生错误

        }
    }

    private void SendCallBack(bool isSuccess, SocketError errorType, string exception)
    {
        //处理发送 成功 或者 失败的逻辑
        Debug.Log("Result : " + isSuccess + " string : " + exception);
    }

    public void PushSendMsg(NetMsgBase msg)
    {
        lock (sendQueue)
        {
            sendQueue.Enqueue(msg);
        }
    }

    public void PushRecvMsg(NetMsgBase msg)
    {
        lock (recvQueue)
        {
            recvQueue.Enqueue(msg);
        }
    }

    void LoopSendMsg()
    {
        while (clientSocket.isConnect())
        {
            lock (sendQueue)
            {
                while (sendQueue.Count > 0)
                {
                    NetMsgBase msg = sendQueue.Dequeue();
                    clientSocket.SendAsyn(msg.GetNetBytes(), SendCallBack);
                }
            }
            Thread.Sleep(100);
        }
    }

   public  void Update()
    {
        if (recvQueue != null)
        {
            if (recvQueue.Count > 0)
            {
                NetMsgBase msg = recvQueue.Dequeue();
                AnalysisMsg(msg);
            }
        }
    }

    void AnalysisMsg(NetMsgBase msg)
    {
        MsgCenter.Instance.HandleMsg(msg);
    }

    private void DisConnectCallBack(bool isSuccess, SocketError SocketError, string exception)
    {
        if (isSuccess)
        {
            sendThread.Abort();
        }
    }

    public void DisConnect()
    {
        if (clientSocket != null && clientSocket.isConnect())
            clientSocket.DisConnectAsyn(DisConnectCallBack);
    }
}
