using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : ManagerBase {

    private static NPCManager _instance;
    public static NPCManager Instance
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
    //存储面板
    private Dictionary<string, GameObject> npcDic = new Dictionary<string, GameObject>();

    public void AddGameObject(string name, GameObject panel)
    {
        if (!npcDic.ContainsKey(name))
        {
            npcDic.Add(name, panel);
        }
    }

    public void RemoveGameObejct(string name)
    {
        if (npcDic.ContainsKey(name))
        {
            npcDic.Remove(name);
        }
    }
    public GameObject GetGameObject(string name)
    {
        if (npcDic.ContainsKey(name))
        {
            return npcDic[name];
        }
        return null;
    }
    /// <summary>
    /// 消息处理 
    /// </summary>
    /// <param name="msg"></param>
    public void AnalysisMsg(MsgBase msg)
    {
        if (msg.GetManagerID() == ManagerID.NpcManager)
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
