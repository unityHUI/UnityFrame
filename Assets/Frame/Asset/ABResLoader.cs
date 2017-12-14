using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABResLoader
{
    private AssetBundle mAB;
    public ABResLoader(AssetBundle tmpAB) {
        mAB = tmpAB;
    }
    /// <summary>
    /// 加载单个资源 
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns></returns>
    public Object LoadSingleRes(string resName) {
        if (mAB != null && mAB.Contains(resName)) {
            return mAB.LoadAsset(resName);
        }
        Debug.Log("res is not Contain");
        return null;
    }
    /// <summary>
    /// 加载带有子物体的资源
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns></returns>
    public Object[] LoadMultRes(string resName) {
        if (mAB != null && mAB.Contains(resName)) {
            return mAB.LoadAssetWithSubAssets(resName);
        }
        Debug.Log("res is not Contain");
        return null;
    }
    /// <summary>
    /// 卸载资源
    /// </summary>
    /// <param name="obj"></param>
    public void UnLoadRes(Object  obj) {
        Resources.UnloadAsset(obj);
    }
    public void DebugAssetsName() {
        string[] nameArr = mAB.GetAllAssetNames();
        for (int i = 0; i < nameArr.Length; i++) {
            Debug.Log(nameArr[i]);
        }
    }

    /// <summary>
    /// 卸载AssetBundle （false） 不卸载从AssetBundle中实例化的所有对象
    /// </summary>
    public void Dispose() {
        if (mAB != null) mAB.Unload(false);
    }


}
