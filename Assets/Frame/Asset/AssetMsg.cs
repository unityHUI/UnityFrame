﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 上层需要发送的消息 
/// </summary>
public class AssetMsg : MsgBase
{
    #region Property
    private string scenceName;
    public string ScenceName
    {
        get
        {
            return scenceName;
        }
    }

    private string bundleName;
    public string BundleName
    {
        get
        {
            return bundleName;
        }
    }

    private string resName;
    public string ResName
    {
        get
        {
            return resName;
        }
    }

    private ushort backMsgId;
    public ushort BackMsgId
    {
        get
        {
            return backMsgId;
        }
    }

    private bool isSingle;
    public bool IsSingle
    {
        get
        {
            return isSingle;
        }
    }
    #endregion

    public AssetMsg(string _scenceName,string _bundleName,string _resName,ushort _backMsgId,ushort _msgId,bool _isSingle) 
        : base(_msgId)
    {
        scenceName = _scenceName;
        bundleName = _bundleName;
        resName = _resName;
        backMsgId = _backMsgId;
        isSingle = _isSingle;
    }
}

/// <summary>
///  Asset加载后返回给上层的消息
/// </summary>
public class AssetBackMsg : MsgBase {
    private Object[] value;
     public Object[] Value
    {
        get
        {
            return value;
        }
    }
    public AssetBackMsg(ushort _msgID,params Object[] _value) : base(_msgID)
    {
        value = _value;
    }
    public void ChangeMsg(ushort _msgID, params Object[] _value) {
        base.MsgID = _msgID;
        value = _value;
    }


}
