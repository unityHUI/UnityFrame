﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : ManagerBase {

    private static NetManager _instance;
    public static NetManager Instance
    {
        get
        {
            return _instance;
        }
    }
    // Use this for initialization
    void Awake()
    {
        _instance = this;
    }
    /// <summary>
    /// 消息处理 
    /// </summary>
    /// <param name="msg"></param>
    public void AnalysisMsg(MsgBase msg)
    {
        if (msg.GetManagerID() == ManagerID.NetManager)
        {
            //本模块直接处理
            HandleMsgEvent(msg);
        }
        else
        {
            //发送到消息中心处理
            MsgCenter.Instance.HandleMsg(msg);
        }
    }
}
