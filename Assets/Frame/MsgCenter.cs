using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgCenter : MonoBehaviour
{
    private static MsgCenter _Instance;
    public static MsgCenter Instance
    {
        get
        {
            return _Instance;
        }
    }
    // Use this for initialization
    void Start()
    {
        _Instance = this;
    }

    public void HandleMsg(MsgBase msg)
    {
        AnalysisMsg(msg);
    }
    private void AnalysisMsg(MsgBase msg)
    {
        ManagerID tmpID = msg.GetManagerID();
        switch (tmpID)
        {
            case ManagerID.UIManager:
                UIManager.Instance.AnalysisMsg(msg);
                break;
            case ManagerID.NpcManager:
                NPCManager.Instance.AnalysisMsg(msg);
                break;
            case ManagerID.GameManager:
                break;
        }
    }
}
