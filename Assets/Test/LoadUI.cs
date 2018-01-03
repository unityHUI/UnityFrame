using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        GameObject loadAsset = UIManager.Instance.GetGameObject("LoadAsset");
        loadAsset.GetComponent<UIBehavier>().AddButtonListener(LoadAssetClick);

        GameObject releseAsset = UIManager.Instance.GetGameObject("ReleseAsset");
        releseAsset.GetComponent<UIBehavier>().AddButtonListener(ReleseAsset);

        GameObject releseBundle = UIManager.Instance.GetGameObject("ReleseBundle");
        releseBundle.GetComponent<UIBehavier>().AddButtonListener(ReleseBundle);

        msgIds = new ushort[] {
          (ushort)UIEventMsg.LoadUIBundleFinish
        };
        RegisterSelfMsg(this, msgIds);
       // AssetMsg assteMsg = new AssetMsg("LoadScence", "LoadModel", "YGHCube", (ushort)UIEventMsg.LoadUIBundleFinish, (ushort)AssetEventMsg.LoadAsset, true);
      //  SendMsg(assteMsg);
    }
    void LoadAssetClick()
    {
        AssetMsg assteMsg = new AssetMsg("LoadScence", "LoadModel", "YGHCube", (ushort)UIEventMsg.LoadUIBundleFinish, (ushort)AssetEventMsg.LoadAsset, true);
        SendMsg(assteMsg);
    }
    void ReleseAsset() {
        AssetMsg assteMsg = new AssetMsg("LoadScence", "LoadTex", "tex", (ushort)UIEventMsg.LoadUIBundleFinish, (ushort)AssetEventMsg.ReleseAsset, true);
        SendMsg(assteMsg);
    }
    void ReleseBundle() {
        AssetMsg assteMsg = new AssetMsg("LoadScence", "LoadModel", null, (ushort)UIEventMsg.LoadUIBundleFinish, (ushort)AssetEventMsg.ReleseBundle,true);
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
        switch (msg.MsgID) {
            case (ushort)UIEventMsg.LoadUIBundleFinish:
                AssetBackMsg tmpMsg = (AssetBackMsg)msg;
                Object[] obj = tmpMsg.Value;
                Instantiate(obj[0]);
                break;
        }
    }
}
