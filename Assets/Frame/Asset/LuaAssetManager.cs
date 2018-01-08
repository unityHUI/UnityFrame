using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;


/// <summary>
/// 一个Lua 加载命令的节点
/// </summary>
public class LuaCallBackNode
{
    public string resName;
    public string bundleName;        //形式 ： 场景文件名/资源文件名  LoadScence/LoadModel.AB(ld)
    public string scenceName;
    public bool isSingle;
    public LuaCallBackNode nextNode;
    public LuaFunction luaFunc;     //记录资源加载完成后要回调的方法 

    public LuaCallBackNode(string _scenceName, string _bundleName, string _resName,
        bool _isSingle, LuaFunction _luaFunc, LuaCallBackNode _nextNode)
    {

        resName = _resName;
        bundleName = _bundleName;
        scenceName = _scenceName;
        isSingle = _isSingle;
        nextNode = _nextNode;
        luaFunc = _luaFunc;
    }

    public void Dispose()
    {
        resName = null;
        bundleName = null;
        scenceName = null;
        nextNode = null;
        luaFunc.Dispose();
    }
}


/// <summary>
/// Lua加载命令节点的管理类
/// </summary>
public class LuaCallBackNodeManager
{
    public Dictionary<string, LuaCallBackNode> nodeDir;
    public LuaCallBackNodeManager()
    {
        nodeDir = new Dictionary<string, LuaCallBackNode>();
    }

    public void AddNode(string bundleName, LuaCallBackNode node)
    {
        if (nodeDir.ContainsKey(bundleName))
        {
            LuaCallBackNode tmpNode = nodeDir[bundleName];
            while (tmpNode.nextNode != null)
            {
                tmpNode = tmpNode.nextNode;
            }
            tmpNode.nextNode = node;
        }
        else
        {
            nodeDir.Add(bundleName, node);
        }
    }
    /// <param name="bundleName"></param>
    public void RemoveBundleNode(string bundleName)
    {
        if (nodeDir.ContainsKey(bundleName))
        {
            LuaCallBackNode currNode = nodeDir[bundleName];
            while (currNode.nextNode != null)
            {
                LuaCallBackNode tmpNode = currNode.nextNode;
                currNode = currNode.nextNode;
                tmpNode.Dispose();
            }
            currNode.Dispose();
            nodeDir.Remove(bundleName);
        }
    }

    public void BundleNodeCallBack(string bundleName)
    {
        if (nodeDir.ContainsKey(bundleName))
        {
            LuaCallBackNode tmpNode = nodeDir[bundleName];
            do
            {
                if (tmpNode.isSingle)
                {
                    Object tmpObj = ABLoadManager.Instance.LoadSingleRes(tmpNode.scenceName, tmpNode.bundleName, tmpNode.resName);
                    tmpNode.luaFunc.Call(tmpNode.scenceName, tmpNode.bundleName, tmpNode.resName, tmpObj);
                }
                else
                {
                    Object[] tmpObjs = ABLoadManager.Instance.LoadMultRes(tmpNode.scenceName, tmpNode.bundleName, tmpNode.resName);
                    tmpNode.luaFunc.Call(tmpNode.scenceName, tmpNode.bundleName, tmpNode.resName, tmpObjs);
                }
                tmpNode = tmpNode.nextNode;
            } while (tmpNode != null);
        }
    }
}
/// <summary>
/// 将框架和 AB Load结合的类  AssetBase 的子类
/// </summary>
public class LuaAssetManager
{
    private static LuaAssetManager instance;
    public static LuaAssetManager Instance
    {
        get
        {
            if (instance == null) instance = new LuaAssetManager();
            return instance;
        }
    }
    private LuaCallBackNodeManager nodeManager;
    public LuaCallBackNodeManager NodeManager
    {
        get
        {
            if (nodeManager == null) nodeManager = new LuaCallBackNodeManager();
            return nodeManager;
        }
    }

    void LoadProgress(string bundleName, float progress)
    {
        Debug.Log("Loading ....  bundleName = " + bundleName);
        if (progress >= 1.0f)
        {
            Debug.Log("Bundle Load Finish bundleName =" + bundleName);
            nodeManager.BundleNodeCallBack(bundleName);
            nodeManager.RemoveBundleNode(bundleName);
        }
    }

    public void LoadAsset(string scenceName, string bundleName, string resName, bool isSingle, LuaFunction luaFunc)
    {
        //当前未加载过 bundle
        if (!ABLoadManager.Instance.IsLoadedBundle(scenceName, bundleName))
        {
            ABLoadManager.Instance.LoadAssetBundle(scenceName, bundleName, LoadProgress);
            string fullName = ABLoadManager.Instance.GetRelateBundleName(scenceName, bundleName);
            // 判断当前是否有该 Bundle
            if (fullName != null)
            {
                LuaCallBackNode node = new LuaCallBackNode(scenceName, bundleName, resName, isSingle, luaFunc, null);
                nodeManager.AddNode(fullName, node);
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
                    LuaCallBackNode node = new LuaCallBackNode(scenceName, bundleName, resName, isSingle,  luaFunc,null);
                    nodeManager.AddNode(fullName, node);
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
                    Object obj = ABLoadManager.Instance.LoadSingleRes(scenceName, bundleName, resName);
                    luaFunc.Call(scenceName, bundleName, resName, obj);
                }
                else
                {
                    Object[] objs = ABLoadManager.Instance.LoadMultRes(scenceName, bundleName, resName);
                    luaFunc.Call(scenceName, bundleName, resName, objs);
                }
            }
        }
    }

}
