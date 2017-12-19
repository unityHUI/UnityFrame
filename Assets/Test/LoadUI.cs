using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUI : UIBase
{
    // Use this for initialization
    void Start()
    {
        GameObject upBtn = UIManager.Instance.GetGameObject("LoadF");
        upBtn.GetComponent<UIBehavier>().AddButtonListener(ForwardClick);
        GameObject backBtn = UIManager.Instance.GetGameObject("LoadB");
        backBtn.GetComponent<UIBehavier>().AddButtonListener(BackClick);
        GameObject leftBtn = UIManager.Instance.GetGameObject("LoadL");
        leftBtn.GetComponent<UIBehavier>().AddButtonListener(LeftClick);
        GameObject rightBtn = UIManager.Instance.GetGameObject("LoadR");
        rightBtn.GetComponent<UIBehavier>().AddButtonListener(RightClick);

        // Test AssetBundle
        msgIds = new ushort[] {
          (ushort)UIEventMsg.LoadUIBundleFinish
        };
        RegisterSelfMsg(this, msgIds);
        AssetMsg assteMsg = new AssetMsg("LoadScence", "LoadTex", "tex", (ushort)UIEventMsg.LoadUIBundleFinish, (ushort)AssetEventMsg.LoadAsset, true);
        SendMsg(assteMsg);
    }
    void ForwardClick() {
        MsgBase msg = new MsgBase((ushort)NPCEvent.Forward);
        SendMsg(msg);
    }
    void BackClick()
    {
        MsgBase msg = new MsgBase((ushort)NPCEvent.Back);
        SendMsg(msg);
    }
    void LeftClick()
    {
        MsgBase msg = new MsgBase((ushort)NPCEvent.Left);
        SendMsg(msg);
    }
    void RightClick()
    {
        MsgBase msg = new MsgBase((ushort)NPCEvent.Right);
        SendMsg(msg);
    }

    private void SendMsg(MsgBase msg) {
       AnalysisMsg(msg);
    }
    public override void HandleMsgEvent(MsgBase msg)
    {

    }
}
