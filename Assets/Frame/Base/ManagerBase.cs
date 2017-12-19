using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ManagerBase : MonoBase
{
    //存储消息链表  key：当前Manager所注册的消息 value:相应当前消息的Script链表
    public Dictionary<ushort, EventNode> eventTreeDic = new Dictionary<ushort, EventNode>();
    /// <summary>
    /// 为某个脚本注册多个消息
    /// </summary>
    /// <param name="mono">要注册消息的脚本</param>
    /// <param name="msgs">注册的消息数组</param>
    public void RegisterMultMsg(MonoBase mono, params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            EventNode node = new EventNode(mono);
            RegisterMsg(node, msgs[i]);
        }
    }
    /// <summary>
    /// 添加节点到消息链表
    /// </summary>
    /// <param name="node">消息节点</param>
    /// <param name="msgID">对应消息链表的ID</param>
    public void RegisterMsg(EventNode node, ushort msgID)
    {
        if (!eventTreeDic.ContainsKey(msgID))
        {
            eventTreeDic.Add(msgID, node);
        }
        else
        {
            EventNode tmpNode = eventTreeDic[msgID];
            while (tmpNode.next != null)
            {
                tmpNode = tmpNode.next;
            }
            tmpNode.next = node;
        }
    }
    /// <summary>
    /// 移除多条消息链中的某个脚本
    /// </summary>
    /// <param name="mono">要移除的脚本</param>
    /// <param name="msgs">需要移除的消息链数组</param>
    public void RemoveMultMsg(MonoBase mono, params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            RemoveMsg(mono, msgs[i]);
        }
    }

    /// <summary>
    /// 移除某消息链中的一个脚本
    /// </summary>
    /// <param name="mono">要移除的脚本</param>
    /// <param name="msgID">要移除对应脚本的消息链ID</param>
    public void RemoveMsg(MonoBase mono, ushort msgID)
    {
        if (!eventTreeDic.ContainsKey(msgID))
        {
            Debug.LogWarning("Dont Contaim msgID" + msgID);
        }
        else
        {
            EventNode tmpNode = eventTreeDic[msgID];
            if (tmpNode.data == mono)
            {
                EventNode headNode = tmpNode;
                if (headNode.next == null)
                {
                    eventTreeDic.Remove(msgID);
                }
                else
                {
                    eventTreeDic[msgID] = headNode.next;
                    headNode.next = null;
                }
            }
            else
            {
                while (tmpNode.next != null && tmpNode.next.data != mono)
                {
                    tmpNode = tmpNode.next;
                }
                if (tmpNode.next.next == null)
                {
                    tmpNode.next = null;
                }
                else
                {
                    EventNode node = tmpNode.next;
                    tmpNode.next = tmpNode.next.next;
                    node.next = null;
                   
                }
            }
        }
    }

    /// <summary>
    /// 接收消息后的处理(本Manager处理该消息)
    /// </summary>
    /// <param name="msg"></param>
    public override void HandleMsgEvent(MsgBase msg)
    {
        if (!eventTreeDic.ContainsKey(msg.MsgID))
        {
            Debug.LogError("Dont Have MsgEvent  ManagerID = " + msg.GetManagerID() + " MsgID =" + msg.MsgID);
        }
        else
        {
            HandOutMsg(msg);
        }
    }
    /// <summary>
    /// 根据消息链进行消息分发
    /// </summary>
    /// <param name="msg"></param>
    private void HandOutMsg(MsgBase msg)
    {
        EventNode tmpNode = eventTreeDic[msg.MsgID];
        while (tmpNode != null)
        {
            tmpNode.data.HandleMsgEvent(msg);
            tmpNode = tmpNode.next;
        }
    }
}

public class EventNode
{

    //当前节点存储的MonoBase 
    public MonoBase data;

    //下一个节点
    public EventNode next;

    public EventNode(MonoBase _data)
    {
        data = _data;
        next = null;
    }
}
