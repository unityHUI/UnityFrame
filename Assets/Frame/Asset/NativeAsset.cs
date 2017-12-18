using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AssetEventMsg {
   Relese = ManagerID.AssetManager + 1,

    ReleseAsset,
    ReleseBundleAsset,
    ReleseScenceAllBundleAsset,
    ReleseAppBundleAsset,

    ReleseBundle,
    ReleseBundleAndAsset,
    ReleseScenceAllBundle,
    ReleseAppBundle,
    MaxValue,
}

public class NativeAsset : AssetBase {

    /// <summary>
    /// 处理释放资源
    /// </summary>
    /// <param name="msg"></param>
    public override void HandleMsgEvent(MsgBase msg)
    {
        AssetMsg tmpMsg = (AssetMsg)msg;
        switch (tmpMsg.MsgID) {
            case (ushort)AssetEventMsg.ReleseAsset:
                ABLoadManager.Instance.ReleseAsset(tmpMsg.ScenceName, tmpMsg.BundleName, tmpMsg.ResName);
                break;
            case (ushort)AssetEventMsg.ReleseBundleAsset:
                ABLoadManager.Instance.ReleseBundleAsset(tmpMsg.ScenceName, tmpMsg.BundleName);
                break;
            case (ushort)AssetEventMsg.ReleseScenceAllBundleAsset:
                ABLoadManager.Instance.ReleseScenceAllBundleAsset(tmpMsg.ScenceName);
                break;
            case (ushort)AssetEventMsg.ReleseAppBundleAsset:
                ABLoadManager.Instance.ReleseAppBundleAsset();
                break;
            case (ushort)AssetEventMsg.ReleseBundle:
                ABLoadManager.Instance.ReleseBundle(tmpMsg.ScenceName, tmpMsg.BundleName,false);
                break;
            case (ushort)AssetEventMsg.ReleseBundleAndAsset:
                ABLoadManager.Instance.ReleseBundle(tmpMsg.ScenceName, tmpMsg.BundleName, true);
                break;
            case (ushort)AssetEventMsg.ReleseScenceAllBundle:
                ABLoadManager.Instance.ReleseScenceAllBundle(tmpMsg.ScenceName);
                break;
            case (ushort)AssetEventMsg.ReleseAppBundle:
                ABLoadManager.Instance.ReleseAppBundle();
                break;
        }
    }

    void Awake() {
        ushort[] msgIds = new ushort[] {
            (ushort)AssetEventMsg.ReleseAsset,
            (ushort)AssetEventMsg.ReleseBundleAsset,
            (ushort)AssetEventMsg.ReleseScenceAllBundleAsset,
            (ushort)AssetEventMsg.ReleseAppBundleAsset,
            (ushort)AssetEventMsg.ReleseBundle,
            (ushort)AssetEventMsg.ReleseBundleAndAsset,
            (ushort)AssetEventMsg.ReleseScenceAllBundle,
            (ushort)AssetEventMsg.ReleseAppBundle,
        };
        RegisterSelfMsg(this, msgIds);
    }


}
