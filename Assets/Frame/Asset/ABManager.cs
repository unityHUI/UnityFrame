using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABManager  {
    Dictionary<string, ABRelativeM> loadHelperDic = new Dictionary<string, ABRelativeM>();




    public void DebugBundleAsset(string bundleName)
    {
        if (loadHelperDic.ContainsKey(bundleName)) {
            loadHelperDic[bundleName].DebugBundleAsset();
        }
    }
    public bool IsLoadFinishBundle(string bundleName) {
        if (loadHelperDic.ContainsKey(bundleName))
        {
            return loadHelperDic[bundleName].IsLoadFinish;
        }
        else{
            Debug.Log("Dont contain Bundle Relative  BundleName =" + bundleName);
            return false;
        }
    }
}
