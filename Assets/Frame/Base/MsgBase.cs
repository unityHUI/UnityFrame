using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgBase
{

    private ushort _msgID;
    /// <summary>
    /// 消息ID
    /// </summary>
    public ushort MsgID
    {
        get
        {
            return _msgID;
        }
    }

    /// <summary>
    /// 获取该消息的ManagerID
    /// </summary>
    /// <returns>ManagerID</returns>
    public ManagerID GetManagerID()
    {
        int temID = _msgID / Tools.MsgSpan;
        return (ManagerID)(temID * Tools.MsgSpan);
    }

    public MsgBase(ushort msgID)
    {
        _msgID = msgID;
    }
}
