using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABLoadManager : MonoBehaviour {
    private static ABLoadManager instance;
    public static ABLoadManager Instance {
        get {
            return instance;
        }
    }
    void Awake() {
        instance = this;
        //加载Manifest 
        StartCoroutine(ManifestManager.Instance.LoadManifest());   
 
    }
    private Dictionary<string, ABScenceManager> abScenceMDir = new Dictionary<string, ABScenceManager>();
    //读取场景 bundle 记录文件
    public void ReadScenceConfig(string scenceName) {
        if (!abScenceMDir.ContainsKey(scenceName)) {
            ABScenceManager abSecenceM = new ABScenceManager(scenceName);
            abSecenceM.ReadConfiger();
            abScenceMDir.Add(scenceName, abSecenceM);
        }
    }


    public void LoadAssetBundle(string scenceName, string bundleName, ABLoadProgress loadProgress) {
        if (!abScenceMDir.ContainsKey(scenceName)) {
            ReadScenceConfig(scenceName);
        }
        ABScenceManager absM = abScenceMDir[scenceName];
        absM.LoadAssetBundle(bundleName, loadProgress, LoadCallBack);
    }
    public void LoadCallBack(string scenceName, string bundleName) {
        if (abScenceMDir.ContainsKey(scenceName))
        {
            ABScenceManager absM = abScenceMDir[scenceName];
            StartCoroutine(absM.LoadAssetBundleSync(bundleName));
        }
        else {
            Debug.Log("There is a Error    " + bundleName);
        }
    }
    /// <summary>
    /// 加载单个资源 
    /// </summary>
    /// <param name="scenceName">场景名称（LoadScence）</param>
    /// <param name="bundleName">场景下分类文件名称（Model）</param>
    /// <param name="resName">具体Asset名称（Cube.prefab）</param>
    public Object LoadSingleRes(string scenceName, string bundleName, string resName) {
        if (abScenceMDir.ContainsKey(scenceName))
        {
            ABScenceManager absM = abScenceMDir[scenceName];
            return absM.GetSingleRes(bundleName, resName);
        }
        else {
            Debug.Log("Dont Have ABScenceManager scenceName =" + scenceName);
            return null;
        }
    }
    /// <summary>
    /// 加载资源 
    /// </summary>
    /// <param name="scenceName">场景名称（LoadScence）</param>
    /// <param name="bundleName">场景下分类文件名称（Model）</param>
    /// <param name="resName">具体Asset名称（Cube.prefab）</param>
    public Object[] LoadMultRes(string scenceName, string bundleName, string resName)
    {
        if (abScenceMDir.ContainsKey(scenceName))
        {
            ABScenceManager absM = abScenceMDir[scenceName];
            return absM.GetMultRes(bundleName, resName);
        }
        else
        {
            Debug.Log("Dont Have ABScenceManager scenceName =" + scenceName);
            return null;
        }
    }

    /// <summary>
    /// 释放某个asset
    /// </summary>
    /// <param name="scenceName"></param>
    /// <param name="bundleName"></param>
    /// <param name="resName"></param>
    public void ReleseAsset(string scenceName,string bundleName, string resName)
    {
        if (abScenceMDir.ContainsKey(scenceName))
        {
            ABScenceManager absM = abScenceMDir[scenceName];
            absM.ReleseAsset(bundleName, resName);
        }
        else
        {
            Debug.Log("Dont Have ABScenceManager scenceName =" + scenceName);
        }
    }
    /// <summary>
    /// 释放某个Bundle加载的所有asset
    /// </summary>
    /// <param name="scenceName"></param>
    /// <param name="bundleName"></param>
    public void ReleseBundleAsset(string scenceName,string bundleName)
    {
        if (abScenceMDir.ContainsKey(scenceName))
        {
            ABScenceManager absM = abScenceMDir[scenceName];
            absM.ReleseBundleAsset(bundleName);
        }
        else
        {
            Debug.Log("Dont Have ABScenceManager scenceName =" + scenceName);
        }
    }
    /// <summary>
    /// 释放单个场景所有bundle 加载的asset
    /// </summary>
    /// <param name="scenceName"></param>
    public void ReleseScenceAllBundleAsset(string scenceName)
    {
        ABScenceManager absM = abScenceMDir[scenceName];
        absM.ReleseAllBundleAsset();
    }
    /// <summary>
    /// 释放整个应用bundle 加载的asset
    /// </summary>
    public void ReleseAppBundleAsset() {
        List<string> keys = new List<string>();
        keys.AddRange(abScenceMDir.Keys);
        for (int i = 0; i < keys.Count; i++) {
            ReleseScenceAllBundleAsset(keys[i]);
        }
    }
    /// <summary>
    /// 释放单个bundle
    /// </summary>
    /// <param name="scenceName"></param>
    /// <param name="bundleName"></param>
    /// <param name="isReleseAsset">是否释放加载出来的asset</param>
    public void ReleseBundle(string scenceName,string bundleName,bool isReleseAsset = true)
    {
        if (abScenceMDir.ContainsKey(scenceName))
        {
            ABScenceManager absM = abScenceMDir[scenceName];
            absM.ReleseBundle(bundleName,isReleseAsset);
        }
        else
        {
            Debug.Log("Dont Have ABScenceManager scenceName =" + scenceName);
        }
    }
    /// <summary>
    /// 释放整个场景的Bundle
    /// </summary>
    /// <param name="scenceName"></param>
    /// <param name="isReleseAsset">是否释放加载出来的asset</param>
    public void ReleseScenceAllBundle(string scenceName, bool isReleseAsset = true)
    {
        if (abScenceMDir.ContainsKey(scenceName))
        {
            ABScenceManager absM = abScenceMDir[scenceName];
            absM.ReleseAllBundle(isReleseAsset);
            System.GC.Collect();
        }
        else {
            Debug.Log("Dont Have ABScenceManager scenceName =" + scenceName);
        }
    }
    /// <summary>
    /// 释放整个应用的Bundle
    /// </summary>
    public void ReleseAppBundle() {
        List<string> keys = new List<string>();
        keys.AddRange(abScenceMDir.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            ReleseScenceAllBundle(keys[i]);
        }
        System.GC.Collect();
    }

    public void DebugAllBundle(string scenceName) {
        if (abScenceMDir.ContainsKey(scenceName))
        {
            ABScenceManager absM = abScenceMDir[scenceName];
            absM.DebugAllBundleInfo();
        }
    }
    void OnDestroy() {
        abScenceMDir.Clear();
    }
}
