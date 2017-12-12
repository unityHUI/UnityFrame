using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase
{
    private static UIManager _instance;
    public static UIManager Instance {
        get {
            return _instance;
        }
    }
    // Use this for initialization
    void Awake()
    {
        _instance = this;
    }
    //存储面板
    private Dictionary<string, GameObject> panelDic = new Dictionary<string, GameObject>();
 
    public void AddGameObject(string name, GameObject panel)
    {
        if (!panelDic.ContainsKey(name))
        {
            panelDic.Add(name, panel);
        }
    }

    public void RemoveGameObejct(string name)
    {
        if (panelDic.ContainsKey(name))
        {
            panelDic.Remove(name);
        }
    }
    public GameObject GetGameObject(string name) {
        if (panelDic.ContainsKey(name)) {
            return panelDic[name];
        }
        return null;
    }
    /// <summary>
    /// 消息处理 
    /// </summary>
    /// <param name="msg"></param>
    public void AnalysisMsg(MsgBase msg)
    {
        if (msg.GetManagerID() == ManagerID.UIManager)
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
