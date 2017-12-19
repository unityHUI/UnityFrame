using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ABLoadProgress(string bundleName, float process);
public delegate void ABLoadFinish(string bundleName);
public class ABLoader
{

    private string bundleName;
    private string bundlePath;
    private float progress;
    private ABResLoader resLoader;
    private ABLoadProgress loadProgressHandler;
    private ABLoadFinish loadFinishHandler;
    private WWW www = null;

    public string BundlePath
    {
        set
        {
            bundlePath = value;
        }
    }
    public ABLoader(ABLoadProgress _loadProgressHandler, ABLoadFinish _loadFinishHandler)
    {
        loadProgressHandler = _loadProgressHandler;
        loadFinishHandler = _loadFinishHandler;
    }
    public void SetBundleName(string _BundleName)
    {
        bundleName = _BundleName;
    }
    public IEnumerator IELoadAB()
    {
        www = new WWW(bundlePath);
        //?????
        while (!www.isDone)
        {
            progress = www.progress;
            if (loadProgressHandler != null)
            {
                loadProgressHandler(bundleName, progress);
            }
            yield return progress;
            progress = www.progress;
        }
        if (progress >= 1.0f)
        {
            resLoader = new ABResLoader(www.assetBundle);
            if (loadProgressHandler != null)
            {
                loadProgressHandler(bundleName, progress);
            }
            if (loadFinishHandler != null)
            {
                loadFinishHandler(bundleName);
            }
        }
        else
        {
            Debug.Log(" AssetBundle Load Error  BundleName = " + bundleName);
        }
        www = null;
    }
    //Debug当前bundle的资源
    public void DebugBundleAsset()
    {
        Debug.Log("BundleName  = " + bundleName + "   AssetList :");
        if (resLoader != null)
            resLoader.DebugAssetsName();
    }
    //加载单个资源
    public Object GetSingleRes(string resName)
    {
        if (resLoader == null) return null;
        return resLoader.LoadSingleRes(resName);
    }

    //加载多个资源 
    public Object[] GetMultRes(string resName)
    {
        if (resLoader == null) return null;
        return resLoader.LoadMultRes(resName);
    }

    //释放资源 
    public void Dispose()
    {
        if (resLoader != null)
        {
            resLoader.Dispose();
            resLoader = null;
        }
    }
    public void UnLoadRes(Object obj)
    {
        if (resLoader != null)
            resLoader.UnLoadRes(obj);
    }
}
