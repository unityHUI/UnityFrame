using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABRelativeM {

    private List<string> dependList;
    private List<string> referList;
    private ABLoader abLoader;
    private string bundleName;
    public string BundleName {
        get {
            return bundleName;
        }
    }
    private ABLoadProgress loadProgress;
    public ABRelativeM() {
        dependList = new List<string>();
        referList = new List<string>();
    }
    private bool isLoadFinish;
    public bool IsLoadFinish {
        get {
            return isLoadFinish;
        }
    }
    public void BundleLoadFinish(string bundleName) {
        isLoadFinish = true;
    }


    public void Initial(string bundleName,ABLoadProgress loadProgress) {
        this.bundleName = bundleName;
        this.loadProgress = loadProgress;
        isLoadFinish = false;
        abLoader = new ABLoader(loadProgress, BundleLoadFinish);
    }

    public List<string> GetReferList()
    {
        return referList;
    }
    public ABLoadProgress GetLoadProgress() {
        return loadProgress;
    }

    public void AddReferBundle(string bundleName) {
        if (!referList.Contains(bundleName)) {
            referList.Add(bundleName);
        }
    }

 
    /// <summary>
    /// 移除被依赖的bundle
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns>true ： 当前无被依赖关系 false ：当前有被依赖关系</returns>
    public bool RemoveReferBundle(string bundleName) {
        for (int i = 0; i < referList.Count; i++) {
            if (bundleName.Equals(referList[i])) {
                referList.RemoveAt(i);
            }
        }
        if (referList.Count <= 0) {
            return true;
        }
        return false;
    }
    public void SetDependList(string[] dependArr) {
        if (dependArr.Length > 0) {
            dependList.AddRange(dependArr);
        }
    }
    public List<string> GetDependList() {
        return dependList;
    }
    public void RemoveDependBundle(string bundleName)
    {
        for (int i = 0; i < referList.Count; i++)
        {
            if (bundleName.Equals(referList[i]))
            {
                referList.RemoveAt(i);
            }
        }  
    }

    // 调用下层API
    public void DebugBundleAsset() {
        if (abLoader != null)
            abLoader.DebugBundleAsset();
    }
    public IEnumerator LoadAssetBundle() {
      yield  return abLoader.IELoadAB();
    }
   //释放资源
    public void Dispose() {
        if (abLoader != null)
            abLoader.Dispose();
    }
    //加载单个资源
    public Object GetSingleRes(string resName)
    {
        if (abLoader == null) return null;
        return abLoader.GetSingleRes(resName);
    }

    //加载多个资源 
    public Object[] GetMultRes(string resName)
    {
        if (abLoader == null) return null;
        return abLoader.GetMultRes(resName);
    }

}

