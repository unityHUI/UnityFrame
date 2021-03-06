﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public delegate void LoadAssetBundleCallBack(string scenceName, string bundleName);
//每一次调用AssetLoad出来的资源 asset
public class Asset
{
    public List<Object> objList;
    //AssetLoad出来的可能是单个Obj 可能是 多个Obj
    public Asset(params Object[] tmpObjs)
    {
        objList = new List<Object>();
        objList.AddRange(tmpObjs);
    }
    public void ReleaseRes()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            Debug.Log("Relese Asset  = "+ objList[i].name);
            if (objList[i].GetType() != typeof(UnityEngine.GameObject)) ;
            Resources.UnloadAsset(objList[i]);
        }
    }
}
//从Bundle中加载的Asset管理类
public class BundleAssetM
{
    //记录所有从该Bundle中AssetLoad出去的Asset
    public Dictionary<string, Asset> resObjs;
    
    public BundleAssetM(string name, Asset obj)
    {
        resObjs = new Dictionary<string, Asset>();
        resObjs.Add(name, obj);
    }
    public void AddAsset(string resName, Asset obj)
    {
        if (!resObjs.ContainsKey(resName))
        {
            resObjs.Add(resName, obj);
        }
    }
    public void ReleseAllAsset()
    {
        List<string> keys = new List<string>();
        keys.AddRange(resObjs.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            ReleseAsset(keys[i]);
        }
    }
    public void ReleseAsset(string name)
    {
        if (resObjs.ContainsKey(name))
        {
            resObjs[name].ReleaseRes();
        }
    }
    public List<Object> GetLoadedAssets(string name)
    {
        if (resObjs.ContainsKey(name))
        {
            return resObjs[name].objList;
        }
        else
        {
            Debug.Log("Get Loaded Assets is Error  bundleName = " + name);
            return null;
        }
    }
}

//管理单个场景的所有Bundle
public class ABManager
{

    private string scenceName;
    public ABManager(string scenceName) {
        this.scenceName = scenceName;
    }
    //存储bundle的依赖管理
    Dictionary<string, ABRelativeM> loadHelperDic = new Dictionary<string, ABRelativeM>();
    //存储场景中所有Bundle的Asset加载管理
    Dictionary<string, BundleAssetM> loadedAssetMDic = new Dictionary<string, BundleAssetM>();

    #region  释放LoadAsset出来的资源
    // 释放单个从Bundle中Load的资源
    public void ReleseLoadedAsset(string bundleName,string resName) {
        if (loadedAssetMDic.ContainsKey(bundleName))
        {
            BundleAssetM baM = loadedAssetMDic[bundleName];
            Debug.Log(" Relese Res  resName =" + resName);
            baM.ReleseAsset(resName);
        }
        else {
            Debug.Log("Dont Have BundleAsset Manager   bundleName =" + bundleName);
        }
    }
    //释放从Bundle中Load的所有资源
    public void ReleseBundleAsset(string bundleName) {
        if (loadedAssetMDic.ContainsKey(bundleName)) {
            BundleAssetM baM = loadedAssetMDic[bundleName];
            baM.ReleseAllAsset();
            Resources.UnloadUnusedAssets();
        }  
    }
    //释放所有Bundle Load的资源
    public void ReleseAllBundleAsset() {
        List<string> keys = new List<string>();
        keys.AddRange(loadedAssetMDic.Keys);
        for (int i = 0; i < keys.Count; i++) {
            ReleseBundleAsset(keys[i]);
        }
        loadedAssetMDic.Clear();
    }

    public void ReleseBundle(string bundleName,bool isReleseRes) {
        if (loadHelperDic.ContainsKey(bundleName)) {
            ABRelativeM abrM = loadHelperDic[bundleName];
            List<string> dependList = abrM.GetDependList();
            for (int i = 0; i < dependList.Count; i++) {
               
                if (loadHelperDic.ContainsKey(dependList[i])){
                    ABRelativeM dependAbrm = loadHelperDic[dependList[i]];
                    if (dependAbrm.RemoveReferBundle(bundleName)) {
                        ReleseBundle(dependAbrm.BundleName,isReleseRes);
                    }
                }
            }
            if (abrM.GetReferList().Count <= 0) {
                if (isReleseRes)
                {
                    ReleseBundleAsset(bundleName);
                }
                abrM.Dispose();
                loadHelperDic.Remove(bundleName);
            }
        }
    }
    public void ReleseAllBundle(bool isRelese) {
        if(isRelese)  ReleseAllBundleAsset();
        List<string> keys = new List<string>();
        keys.AddRange(loadHelperDic.Keys);
        for (int i = 0; i < keys.Count; i++) {
            loadHelperDic[keys[i]].Dispose();
        }
        loadHelperDic.Clear();
    }
    #endregion

    private string[] GetBundleDepends(string bundleName) {
        return ManifestManager.Instance.GetBundleDepends(bundleName);
    }
    public void LoadAssetBundle(string bundleName, ABLoadProgress loadProgress, LoadAssetBundleCallBack callBack) {
        if (!loadHelperDic.ContainsKey(bundleName)) {
            ABRelativeM abM = new ABRelativeM();
            abM.Initial(bundleName, loadProgress);
            loadHelperDic.Add(bundleName, abM);
            callBack(scenceName, bundleName);
        }
    }

    public IEnumerator LoadABDepends(string bundleName, string refName, ABLoadProgress loadProgress) {
        if (!loadHelperDic.ContainsKey(bundleName))
        {
            ABRelativeM abM = new ABRelativeM();
            abM.Initial(bundleName, loadProgress);
            loadHelperDic.Add(bundleName, abM);
            if (refName != null)
            {
                abM.AddReferBundle(refName);
            }
            yield return LoadAssetBundle(bundleName);
        }
        else {
            if(refName != null) {
                loadHelperDic[bundleName].AddReferBundle(refName);
            }
        }
    }

    public IEnumerator LoadAssetBundle(string bundleName) {
        while (!ManifestManager.Instance.IsLoadFinish) {
            yield return null;
        }
        ABRelativeM abM = loadHelperDic[bundleName];
        string[] depends = GetBundleDepends(bundleName);
        abM.SetDependList(depends);
        for (int i = 0; i < depends.Length; i++) {
            yield return LoadABDepends(depends[i], bundleName, abM.GetLoadProgress());
        }
       
        yield return abM.LoadAssetBundle();

    }

    /// <summary>
    /// 表示是否加载了Bundle 
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public bool  IsLoadedBundle(string bundleName)
    {
        if (loadHelperDic.ContainsKey(bundleName)) {
            return true;
        }
        return false;
    }

    //调用底层API
    public void DebugBundleAsset(string bundleName)
    {
        if (loadHelperDic.ContainsKey(bundleName))
        {
            loadHelperDic[bundleName].DebugBundleAsset();
        }
    }
    /// <summary>
    /// 表示bundle 是否加载完成
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public bool IsLoadFinishBundle(string bundleName)
    {
        if (loadHelperDic.ContainsKey(bundleName))
        {
            return loadHelperDic[bundleName].IsLoadFinish;
        }
        else
        {
            Debug.Log("Dont contain Bundle Relative  BundleName =" + bundleName);
            return false;
        }
    }

    public Object GetSingleRes(string bundleName, string resName)
    {
        //是否缓存了物体 
        if (loadedAssetMDic.ContainsKey(bundleName))
        {
            List<Object> tmpList = loadedAssetMDic[bundleName].GetLoadedAssets(resName);
            Debug.Log("已缓存Asset  = "+ tmpList[0].name);
            if (tmpList != null) return tmpList[0];
        }
        //是否加载过Bundle 
        if (loadHelperDic.ContainsKey(bundleName))
        {
            ABRelativeM abM = loadHelperDic[bundleName];
            Debug.Log("从已缓存bundle中加载 bundleName = " + bundleName + " Asset =" + resName);
            Object tmpObj = abM.GetSingleRes(resName);
            Asset assetObj = new Asset(tmpObj);
            //如果当前已经存在Bundle的BundleAssetM
            if (loadedAssetMDic.ContainsKey(bundleName))
            {
                BundleAssetM assetM = loadedAssetMDic[bundleName];
                assetM.AddAsset(resName, assetObj);
            }
            //如果当前不存在该Bundle的BundleAssetM
            else
            {
                BundleAssetM assetM = new BundleAssetM(resName, assetObj);
                loadedAssetMDic.Add(bundleName, assetM);
            }
            return tmpObj;
        }
        else {
            Debug.Log("Dont Load the Bundle    bundleName = " + bundleName);
            return null;
        }

    }

    public Object[] GetMultRes(string bundleName, string resName)
    {
        //是否缓存了物体 
        if (loadedAssetMDic.ContainsKey(bundleName))
        {
            List<Object> tmpList = loadedAssetMDic[bundleName].GetLoadedAssets(resName);
            if (tmpList != null) return tmpList.ToArray();
        }
        //是否加载过Bundle 
        if (loadHelperDic.ContainsKey(bundleName))
        {
            ABRelativeM abM = loadHelperDic[bundleName];
            Object[] tmpObj = abM.GetMultRes(resName);
            Asset assetObj = new Asset(tmpObj);
            //如果当前已经缓存过 该 bundleName 的Res 
            if (loadedAssetMDic.ContainsKey(bundleName))
            {
                BundleAssetM assetM = loadedAssetMDic[bundleName];
                assetM.AddAsset(resName, assetObj);
            }
            //当前未缓存过该bundleName的包
            else
            {
                BundleAssetM assetM = new BundleAssetM(resName, assetObj);
                loadedAssetMDic.Add(bundleName, assetM);
            }
            return tmpObj;
        }
        else
        {
            Debug.Log("Dont Load the Bundle    bundleName = " + bundleName);
            return null;
        }
    }

   
}
