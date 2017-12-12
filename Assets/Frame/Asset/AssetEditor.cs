using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetEditor
{
    [MenuItem("Tools/AssetBundle")]
    public static void MarkAssetBundle()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        string path = Global.AssetBundlePath;
        DirectoryInfo tempDir = new DirectoryInfo(path);
        FileSystemInfo[] filesInfo = tempDir.GetFileSystemInfos();
        for (int i = 0; i < filesInfo.Length; i++)
        {
            FileSystemInfo tmpFile = filesInfo[i];
            if (tmpFile is DirectoryInfo)
            {
                string tempPath = Path.Combine(path, tmpFile.Name);
                LoadScenceBundle(tempPath);
            }
        }
    }
    //获取到资源的场景路径，对该场景的资源进行打包
    public static void LoadScenceBundle(string scencePath)
    {
        string txtFileName = "Record.txt";
        string tmpPath = scencePath + txtFileName;
        FileStream fs = new FileStream(tmpPath, FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(fs);
        
        Dictionary<string, string> readDir = new Dictionary<string, string>();
        GetRelativePath(tmpPath, readDir);
        sw.Close();
        fs.Close();
    }

    public static void GetRelativePath(string fullPath,Dictionary<string,string> writer) {

    }
}
