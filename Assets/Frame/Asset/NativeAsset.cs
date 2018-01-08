using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AssetEventMsg
{
    Relese = ManagerID.AssetManager + 1,

    ReleseAsset,
    ReleseBundleAsset,
    ReleseScenceAllBundleAsset,
    ReleseAppBundleAsset,

    ReleseBundle,
    ReleseBundleAndAsset,
    ReleseScenceAllBundle,
    ReleseAppBundle,

    LoadAsset,
    MaxValue,
}

public delegate void NativeAssetCallBack(NativeAssetCommandNode commandNode);

// load Asset的 命令类 
public class NativeAssetCommandNode
{
    public string scenceName;
    public string bundleName;
    public string resName;
    public bool isSingle;
    //回调的msgID
    public ushort backId;
    public NativeAssetCallBack callBack;
    public NativeAssetCommandNode next;

    //构造函数
    public NativeAssetCommandNode(string _scenceName, string _bundleName, string _resName,
        bool _isSingle, ushort _backId, NativeAssetCallBack _callBack, NativeAssetCommandNode _next)
    {
        scenceName = _scenceName;
        bundleName = _bundleName;
        resName = _resName;
        isSingle = _isSingle;
        backId = _backId;
        callBack = _callBack;
        next = _next;
    }
    public void Dispose()
    {
        scenceName = null;
        bundleName = null;
        resName = null;
        callBack = null;
        next = null;
    }
}
// Load Res 的命令管理类
public class NativeAssetCommandManager
{
    public Dictionary<string, NativeAssetCommandNode> commandDir;
    public NativeAssetCommandManager()
    {
        commandDir = new Dictionary<string, NativeAssetCommandNode>();
    }

    public void AddCommand(string bundleName, NativeAssetCommandNode node)
    {
        if (commandDir.ContainsKey(bundleName))
        {
            NativeAssetCommandNode tmpNode = commandDir[bundleName];
            while (tmpNode.next != null)
            {
                tmpNode = tmpNode.next;
            }
            tmpNode.next = node;
        }
        else
        {
            commandDir.Add(bundleName, node);
        }
    }
    /// <summary>
    /// 加载完成，并向上层发送消息后，移除bundle对应缓存的所有命令
    /// </summary>
    /// <param name="bundleName"></param>
    public void RemoveBundleCommand(string bundleName)
    {
        if (commandDir.ContainsKey(bundleName))
        {
            NativeAssetCommandNode currNode = commandDir[bundleName];
            while (currNode.next != null)
            {
                NativeAssetCommandNode tmpNode = currNode.next;
                currNode = currNode.next;
                tmpNode.Dispose();
            }
            currNode.Dispose();
            commandDir.Remove(bundleName);
        }
    }

    public void CommandCallBack(string bundleName)
    {
        if (commandDir.ContainsKey(bundleName))
        {
            NativeAssetCommandNode tmpNode = commandDir[bundleName];
            do
            {
                tmpNode.callBack(tmpNode);
                tmpNode = tmpNode.next;
            } while (tmpNode != null);
        }
    }
}
public class NativeAsset : AssetBase
{
    private NativeAssetCommandManager commandManager;
    public NativeAssetCommandManager CommandManager
    {
        get
        {
            if (commandManager == null) commandManager = new NativeAssetCommandManager();
            return commandManager;
        }
    }

    private AssetBackMsg currBackMsg;
    public AssetBackMsg CurrBackMsg
    {
        get
        {
            if (currBackMsg == null) currBackMsg = new AssetBackMsg(0, null);
            return currBackMsg;
        }
    }

    /// <summary>
    /// 处理释放资源
    /// </summary>
    /// <param name="msg"></param>
    public override void HandleMsgEvent(MsgBase msg)
    {
        AssetMsg tmpMsg = (AssetMsg)msg;
        switch (tmpMsg.MsgID)
        {
            case (ushort)AssetEventMsg.ReleseAsset:
                Debug.Log("ReleseAsset name =" + tmpMsg.ResName);
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
                ABLoadManager.Instance.ReleseBundle(tmpMsg.ScenceName, tmpMsg.BundleName, false);
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
            case (ushort)AssetEventMsg.LoadAsset:
                LoadAsset(tmpMsg.ScenceName, tmpMsg.BundleName, tmpMsg.ResName, tmpMsg.IsSingle, tmpMsg.BackMsgId);
                break;
        }
    }

    void Awake()
    {
        ushort[] msgIds = new ushort[] {
            (ushort)AssetEventMsg.ReleseAsset,
            (ushort)AssetEventMsg.ReleseBundleAsset,
            (ushort)AssetEventMsg.ReleseScenceAllBundleAsset,
            (ushort)AssetEventMsg.ReleseAppBundleAsset,
            (ushort)AssetEventMsg.ReleseBundle,
            (ushort)AssetEventMsg.ReleseBundleAndAsset,
            (ushort)AssetEventMsg.ReleseScenceAllBundle,
            (ushort)AssetEventMsg.ReleseAppBundle,
            (ushort)AssetEventMsg.LoadAsset,
        };
        RegisterSelfMsg(this, msgIds);
    }

    public void SendBackMsg(NativeAssetCommandNode node)
    {
        if (node.isSingle)
        {
            UnityEngine.Object obj = ABLoadManager.Instance.LoadSingleRes(node.scenceName, node.bundleName, node.resName);
            CurrBackMsg.ChangeMsg(node.backId, obj);
        }
        else
        {
            UnityEngine.Object[] objs = ABLoadManager.Instance.LoadMultRes(node.scenceName, node.bundleName, node.resName);
            CurrBackMsg.ChangeMsg(node.backId, objs);
        }
        AnalysisMsg(CurrBackMsg);
    }

    void LoadProgress(string bundleName, float progress)
    {
        Debug.Log("Loading ....  bundleName = " + bundleName);
        if (progress >= 1.0f)
        {
            Debug.Log("Bundle Load Finish bundleName =" + bundleName);
            commandManager.CommandCallBack(bundleName);
            commandManager.RemoveBundleCommand(bundleName);
        }
    }

    public void LoadAsset(string scenceName, string bundleName, string resName, bool isSingle, ushort backId)
    {
        //当前未加载过 bundle
        if (!ABLoadManager.Instance.IsLoadedBundle(scenceName, bundleName))
        {
            ABLoadManager.Instance.LoadAssetBundle(scenceName, bundleName, LoadProgress);
            string fullName = ABLoadManager.Instance.GetRelateBundleName(scenceName, bundleName);
            // 判断当前是否有该 Bundle
            if (fullName != null)
            {
                NativeAssetCommandNode node = new NativeAssetCommandNode(scenceName, bundleName, resName, isSingle, backId, SendBackMsg, null);
                CommandManager.AddCommand(fullName, node);
            }
            else
            {
                Debug.Log("The Scence Bundle Txt Don't have bundle bundleName =" + bundleName);
            }
        }
        else
        {
            bool isLoadFinish = ABLoadManager.Instance.IsLoadingFinish(scenceName, bundleName);
            // 资源正在加载中  
            if (!isLoadFinish)
            {
                string fullName = ABLoadManager.Instance.GetRelateBundleName(scenceName, bundleName);
                // 判断当前是否有该 Bundle
                if (fullName != null)
                {
                    NativeAssetCommandNode node = new NativeAssetCommandNode(scenceName, bundleName, resName, isSingle, backId, SendBackMsg, null);
                    CommandManager.AddCommand(fullName, node);
                }
                else
                {
                    Debug.Log("The Scence Bundle Txt Don't have bundle bundleName =" + bundleName);
                }
            }
            //资源已经加载过 处于缓存中 
            else
            {
                if (isSingle)
                {
                    UnityEngine.Object obj = ABLoadManager.Instance.LoadSingleRes(scenceName, bundleName, resName);
                    CurrBackMsg.ChangeMsg(backId, obj);
                }
                else
                {
                    UnityEngine.Object[] objs = ABLoadManager.Instance.LoadMultRes(scenceName, bundleName, resName);
                    CurrBackMsg.ChangeMsg(backId, objs);
                }
                AnalysisMsg(CurrBackMsg);
            }
        }
    }
}
