using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManifestManager  {
    private static ManifestManager instance;
    public static ManifestManager Instance {
        get {
            if (instance == null) {
                instance = new ManifestManager();
            }
            return instance;
        }
    }

    private string manifestPath;
    public string ManifestPath {
        get {
            if (manifestPath == null) {
                manifestPath = PathTool.GetBundlePath();
            }
            return manifestPath;
        }
    }
    private bool isLoadFinish;
    public bool IsLoadFinish {
        get {
            return isLoadFinish;
        }
    }
    public AssetBundleManifest manifest;
    public AssetBundle manifestBundle;
    public ManifestManager() {
        manifestPath = "";
        isLoadFinish = false;  
    }

    public IEnumerator LoadManifest() {
        WWW www = new WWW(manifestPath);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            manifestBundle = www.assetBundle;
            manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            isLoadFinish = true;
        }
        else {
            Debug.Log("Load Manifest Error   Error =" + www.error);
        }
    }
    public string[] GetBundleDepends(string bundleName) {
        return manifest.GetAllDependencies(bundleName);
    }
    public void UnLoadManifest() {
        manifestBundle.Unload(true);
    }
    
}
