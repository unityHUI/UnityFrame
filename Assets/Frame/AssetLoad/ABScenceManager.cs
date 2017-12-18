using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ABScenceManager  {

    private ABManager abManager;
    private string scenceName;
    public ABScenceManager(string scenceName) {
        abManager = new ABManager(scenceName);
        this.scenceName = scenceName;
    }
    private Dictionary<string, string> allBundleDir = new Dictionary<string, string>();

    public void ReadConfiger() {
        string txtFileName = "Record.txt";
        string path = PathTool.GetBundlePath() + "/" + scenceName + txtFileName;
        ReadRecordTxt(path);
    }
    private void ReadRecordTxt(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        string tmpStr = sr.ReadLine();
        if (tmpStr != null)
        {
            string[] tmpstrArr = tmpStr.Split(" - ".ToCharArray());
            allBundleDir.Add(tmpstrArr[0], tmpstrArr[1]);
        }
        sr.Close();
        fs.Close();
    }

    /// <summary>
    ///  对应RecordTxt  前面为bundleKey， 后面为bundleName
    /// </summary>
    /// <param name="bundleKey"></param>
    /// <param name=""></param>
    public void LoadAssetBundle(string bundleKey, ABLoadProgress loadProgress, LoadAssetBundleCallBack callBack) {
        if (allBundleDir.ContainsKey(bundleKey))
        {
            string bundleName = allBundleDir[bundleKey];
            abManager.LoadAssetBundle(bundleName, loadProgress, callBack);
        }
        else {
            Debug.Log("Dont have Bundle  bundleName = " + bundleKey);
        }
    }

    public IEnumerator LoadAssetBundleSync(string bundleName) {
        yield return abManager.LoadAssetBundle(bundleName);
    }
    public Object GetSingleRes(string bundleKey, string resName) {
        if (allBundleDir.ContainsKey(bundleKey))
        {
            return abManager.GetSingleRes(allBundleDir[bundleKey], resName);
        }
        else {
            Debug.Log("Dont have Bundle  bundleKey = " + bundleKey);
            return null;
        }      
    }
    public Object[] GetMultRes(string bundleKey, string resName)
    {
        if (allBundleDir.ContainsKey(bundleKey))
        {
            return abManager.GetMultRes(allBundleDir[bundleKey], resName);
        }
        else
        {
            Debug.Log("Dont have Bundle  bundleKey = " + bundleKey);
            return null;
        }
    }
    public void ReleseAsset(string bundleKey,string resName) {
        if (allBundleDir.ContainsKey(bundleKey))
        {
           abManager.ReleseLoadedAsset(allBundleDir[bundleKey], resName);
        }
        else
        {
            Debug.Log("Dont have Bundle  bundleKey = " + bundleKey);
        }
    }
    public void ReleseBundleAsset(string bundleKey)
    {
        if (allBundleDir.ContainsKey(bundleKey))
        {
            abManager.ReleseBundleAsset(allBundleDir[bundleKey]);
        }
        else
        {
            Debug.Log("Dont have Bundle  bundleKey = " + bundleKey);
        }
    }
    public void ReleseAllBundleAsset()
    {
        abManager.ReleseAllBundleAsset();
    }
    public void ReleseBundle(string bundleKey,bool isReleseAsset) {
        if (allBundleDir.ContainsKey(bundleKey))
        {
            abManager.ReleseBundle(allBundleDir[bundleKey],isReleseAsset);
        }
        else {
            Debug.Log("Dont have Bundle  bundleKey = " + bundleKey);
        }
    }
    public void ReleseAllBundle(bool isReleseAsset) {
        abManager.ReleseAllBundle(isReleseAsset);
        allBundleDir.Clear();
    }

    public void DebugAllBundleInfo() {
        List<string> keys = new List<string>();
        keys.AddRange(allBundleDir.Keys);
        for (int i = 0; i < keys.Count; i++) {
            abManager.DebugBundleAsset(keys[i]);
        }
    }

}
