using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void CallBackRecvOver(byte[] allData);
public class SocketBuffer {
    private byte[] headByte;
    private byte headLength;
    private byte[] allRevData;
    private int curRevLength;
    private int allDataLength;

    public SocketBuffer(byte tmpHeadLength,CallBackRecvOver _callback) {
        headLength = tmpHeadLength;
        headByte = new byte[headLength];
        callBack = _callback;
    }

    public void RevByte(byte[] revByte, int realLength) {
        Debug.Log("Handle Socket Msg");
        if (realLength == 0) return;
        if (curRevLength < headByte.Length)
        {
            RevHead(revByte, realLength);
        }
        else {
            int tmpLength = curRevLength + realLength;
            if (tmpLength == allDataLength)
            {
                RevAll(revByte, realLength);
            }
            else if (tmpLength > allDataLength)
            {
                RevBigger(revByte, realLength);
            }
            else {
                RevSmaller(revByte, realLength);
            }
        }
    }
    private void RevBigger(byte[] revByte, int realLength) {
        int tmpLength = allDataLength - curRevLength;
        Buffer.BlockCopy(revByte, 0, allRevData, curRevLength, tmpLength);
        curRevLength += tmpLength;
        RecvMsgOver();

        int remainLength = realLength - tmpLength;
        byte[] remainByte = new byte[remainLength];
        Buffer.BlockCopy(revByte, tmpLength, remainByte, 0, remainLength);
        RevByte(remainByte, remainLength);
    }
    private void RevSmaller(byte[] revByte, int realLength) {
        Buffer.BlockCopy(revByte, 0, allRevData, curRevLength, realLength);
        curRevLength += realLength;
    }
    private void RevAll(byte[] revByte, int realLength) {
        Buffer.BlockCopy(revByte, 0, allRevData, curRevLength, realLength);
        curRevLength += realLength;
        RecvMsgOver();
    }
    private void RevHead(byte[] recByte, int realLength) {
        int tmpReal = headByte.Length - curRevLength;
        int tmpLength = curRevLength + realLength;
        if (tmpLength < headByte.Length)
        {
            Buffer.BlockCopy(recByte, 0, headByte, curRevLength, realLength);
            curRevLength += tmpReal;
        }
        else {
            Buffer.BlockCopy(recByte, 0, headByte, curRevLength, tmpReal);
            curRevLength += tmpReal;

           
            allDataLength = BitConverter.ToInt32(headByte, 0) + headLength;
            allRevData = new byte[allDataLength];
            Buffer.BlockCopy(headByte, 0, allRevData, 0, headLength);
            int tmpRemain = realLength - tmpReal;
            if (tmpRemain > 0)
            {
                byte[] tmpByte = new byte[tmpRemain];
                Buffer.BlockCopy(recByte, tmpReal, tmpByte, 0, tmpRemain);
                RevByte(tmpByte, tmpRemain);
            }
            else {
                RecvMsgOver();
            }
        }
    }
    public delegate void CallBackRecvOver(byte[] allData);
    CallBackRecvOver callBack;
    private void RecvMsgOver() {
        if (callBack != null) {
        
            callBack(allRevData);
        }
        Debug.Log("CallBack Did");

        curRevLength = 0;
        allDataLength = 0;
        allRevData = null;
    }
}

