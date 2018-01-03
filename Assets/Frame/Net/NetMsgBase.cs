using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NetMsgBase :MsgBase {
    private byte[] msgBytes;

    public NetMsgBase(byte[] bytes) {
        msgBytes = bytes;
        this.MsgID = BitConverter.ToUInt16(bytes, 4);
    }
    
    public byte[] GetNetBytes()
    {
        return msgBytes;
    }

}
