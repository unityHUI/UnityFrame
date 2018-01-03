using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
public enum SocketError
{
    Success = 0,
    TimeOut,
    SocketNull,
    ConnectError,
    SendError,
    RecvError,
    DisConnectError,
}
public class NetSocket  {

    public delegate void NormalCallBack(bool success, SocketError SocketError, string exception);
    public delegate void RecvCallBack(bool success, SocketError SocketError, string exception, byte[] msg, string str);

    private NormalCallBack connectBack;
    private NormalCallBack sendBack;
    private NormalCallBack disConnectBack;
    private RecvCallBack recvBack;

    private SocketError SocketError;
    private Socket clientSocket;
    private string ip;
    private ushort port;
    private SocketBuffer socketBuffer;

    public NetSocket() {
        socketBuffer = new SocketBuffer(6, RecvMsgOver);
        recvBytes = new byte[1024];
    }

    public bool isConnect() {
        return (clientSocket != null && clientSocket.Connected);
    }

    #region Connect
    public void AsynConnect(string ip, ushort port, NormalCallBack connectBack, RecvCallBack recvBack) {
        SocketError = SocketError.Success;
        this.connectBack = connectBack;
        this.recvBack = recvBack;
        if (clientSocket != null) {
            if (clientSocket.Connected)
            {
                this.connectBack(false, SocketError.ConnectError, "Connect Repeat");
            }
            else {
                IPAddress ipA = IPAddress.Parse(ip);
                IPEndPoint point = new IPEndPoint(ipA, port);
                clientSocket.BeginConnect(point, ConnectCallBack, clientSocket);
            }
        } else {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipA = IPAddress.Parse(ip);
            IPEndPoint point = new IPEndPoint(ipA, port);
            IAsyncResult ar = clientSocket.BeginConnect(point, ConnectCallBack, clientSocket);
            TimeOutCheck(ar);
        }
    }

    private void ConnectCallBack(IAsyncResult ar) {
        try
        {
            clientSocket.EndConnect(ar);
            if (clientSocket.Connected == false)
            {
                SocketError = SocketError.ConnectError;
                this.connectBack(false, SocketError, "Unknown Error");
                return;
            }
            else {
                //连接成功
            }

        }
        catch(Exception e)
        {
            this.connectBack(false, SocketError.ConnectError, e.ToString());
        }
    }
    #endregion

    #region Receive
    private byte[] recvBytes;
    public void ReceiveAsyn() {
        if (clientSocket != null && clientSocket.Connected)
        {
        IAsyncResult ar =     clientSocket.BeginReceive(recvBytes, 0, recvBytes.Length, SocketFlags.None, ReceiveCallBack, clientSocket);
            if (!TimeOutCheck(ar)) {
                recvBack(false, SocketError.TimeOut, "连接超时",null,"");
            }
        }
        else {
            recvBack(false, SocketError.RecvError, "Socket为空或者未连接", null, "");
        }
    }
    private void ReceiveCallBack(IAsyncResult ar) {
        try
        {
            if (clientSocket == null || !clientSocket.Connected) {
                recvBack(false, SocketError.RecvError, "Socket为空或者未连接", null, "");
                return;
            }
            int length = clientSocket.EndReceive(ar);
            if (length == 0) return;
            socketBuffer.RevByte(recvBytes, length);
        }
        catch (Exception e) {
            recvBack(false, SocketError.RecvError, e.ToString(), null, "");
        }
        ReceiveAsyn();
    }

    public void RecvMsgOver(byte[] allbyte)
    {
        recvBack(true, SocketError.Success, "接收成功", allbyte, "");
    }
    #endregion

    #region Send
    public void SendAsyn(byte[] sendBytes, NormalCallBack sendCallBack) {
        this.sendBack = sendCallBack;
        if (clientSocket != null && clientSocket.Connected)
        {
           IAsyncResult ar = clientSocket.BeginSend(sendBytes, 0, sendBytes.Length, SocketFlags.None, SendCallBack, clientSocket);
            if (!TimeOutCheck(ar)) {
                sendBack(false, SocketError.TimeOut, "发送时间超时");
            }
        }
        else {
            sendBack(false, SocketError.SendError, "Socket为空或者未连接");
        }
    }

    private void SendCallBack(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || !clientSocket.Connected)
            {
                sendBack(false, SocketError.SendError, "Socket为空或者未连接");
                return;
            }
            int length = clientSocket.EndSend(ar);
            if (length > 0)
            {
                sendBack(true, SocketError.Success, "");
            }        
        }
        catch (Exception e)
        {
            sendBack(false, SocketError.SendError, e.ToString());
        }
    }

    #endregion

    #region DisConnect
    public void DisConnectAsyn(NormalCallBack disCallBack) {
        this.disConnectBack = disCallBack;
        if (clientSocket != null && clientSocket.Connected)
        {
          IAsyncResult ar =  clientSocket.BeginDisconnect(false, DisConnectCallBack, clientSocket);
            if (!TimeOutCheck(ar))
            {
                disCallBack(false, SocketError.TimeOut, "发送时间超时");
            }
        }
        else
        {
            disCallBack(false, SocketError.DisConnectError, "Socket为空或者未连接");
        }
    }
    private void DisConnectCallBack(IAsyncResult ar) {
        try
        {
            clientSocket.EndDisconnect(ar);
            clientSocket.Close();
            clientSocket = null;
            disConnectBack(true, SocketError.Success, "");
        }
        catch (Exception e) {
            disConnectBack(false, SocketError.DisConnectError, e.ToString());
        }
    }

    #endregion

    #region TimeOut Check
    bool TimeOutCheck(IAsyncResult ar) {
        int i = 0;
        while (ar.IsCompleted == false) {
            i++;
            if (i > 5) {
                SocketError = SocketError.TimeOut;
                this.connectBack(false, SocketError.TimeOut, "Connect Time Too Long");
                return false;
            }
            Thread.Sleep(1000);
        }
        return true;
    }
    #endregion;
}
