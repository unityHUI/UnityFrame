﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCBase : MonoBase {

    public ushort[] msgIds;
    /// <summary>
    /// 将脚本添加到UIManager的消息响应链中
    /// </summary>
    /// <param name="mono">脚本</param>
    /// <param name="msgs">脚本要监听的消息</param>
    public void RegisterSelfMsg(MonoBase mono, params ushort[] msgs)
    {
        NPCManager.Instance.RegisterMultMsg(mono, msgs);
    }
    /// <summary>
    /// 将脚本从UIManager的消息链中移除
    /// </summary>
    /// <param name="mono">脚本</param>
    /// <param name="msgs">脚本监听的消息</param>
    public void RemoveSelfMsg(MonoBase mono, params ushort[] msgs)
    {
        NPCManager.Instance.RemoveMultMsg(mono, msgs);
    }
    public override void HandleMsgEvent(MsgBase msg)
    {
        
    }
    void OnDestroy()
    {
        if (msgIds != null)
        {
            RemoveSelfMsg(this, msgIds);
        }
    }
}
