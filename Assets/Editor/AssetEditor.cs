using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetEditor
{
    [MenuItem("Tools/BuildAssetBundle")]
    public static void BuildAssetBundle() {
        string assetBundleDirectory = PathTool.GetBundlePath();
       
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
    /// <summary>
    /// 遍历 D :UnityFrame/Assets/Art/Scences/ 目录，添加下一级的目录
    /// </summary>
    [MenuItem("Tools/MarkAssetBundle")]
    public static void MarkAssetBundle()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        string path = Global.AssetBundlePath;
        DirectoryInfo tempDir = new DirectoryInfo(path);
        Debug.Log(tempDir.FullName);
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
    /// <summary>
    /// 添加标记文档 
    /// </summary>
    /// <param name="scencePath"></param>
    // D :UnityFrame/Assets/Art/Scences/LoadScence
    public static void LoadScenceBundle(string scencePath)
    {
        string txtFileName = "Record.txt";
        string tmpPath = scencePath + txtFileName;
        FileStream fs = new FileStream(tmpPath, FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(fs);      
        Dictionary<string, string> readDir = new Dictionary<string, string>();
        GetRelativePath(scencePath, readDir);
        foreach (string key in readDir.Keys) {
            sw.WriteLine(key + " - " + readDir[key]);
        }
        sw.Close();
        fs.Close();
    }
    /// <summary>
    ///  截取相对路径
    /// </summary>
    /// <param name="fullPath">D :UnityFrame/Assets/Art/Scences/LoadScence</param>
    /// <param name="writer"></param>
    /// Art/Scences/LoadScence
    public static void GetRelativePath(string fullPath,Dictionary<string,string> writer) {
        int tmpCount = fullPath.IndexOf("Assets");
        string relativePath = fullPath.Substring(tmpCount, fullPath.Length - tmpCount);
        DirectoryInfo dir = new DirectoryInfo(fullPath);
        if (dir != null)
        {
            ListFiles(dir, relativePath, writer);
        }
        else {
            Debug.Log("The path is not exists   path =" + fullPath);
        }
    }
    /// <summary>
    /// 遍历每一个场景文件夹，对每一个文件进行遍历 
    /// </summary>
    /// <param name="info"> D :UnityFrame/Assets/Art/Scences/LoadScence </param>
    /// <param name="relativePath">Art/Scences/LoadScence</param>
    /// <param name="writer"></param>
    public static void ListFiles(FileSystemInfo info, string relativePath, Dictionary<string, string> writer) {
        if (!info.Exists) {
            Debug.Log("path is not Exist  Path =" + info);
            return;
        }
       
        DirectoryInfo dir = info as DirectoryInfo;
        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++) {
            FileInfo file = files[i] as FileInfo;
            if (file != null)
            {
              //  D: UnityFrame / Assets / Art / Scences / LoadScence / LoadUI / Image.prefab
                ChangeMark(file, relativePath, writer);
            }
            else {
                ListFiles(files[i], relativePath, writer);
            }
        }
    }
    public static string FixedPath(string path) {
        path = path.Replace("\\","/");
        return path;
    }
    /// <summary>
    /// 获取每个具体文件的打包标记 只到二级目录
    /// </summary>
    /// <param name="file">D: UnityFrame / Assets / Art / Scences / LoadScence / LoadUI / Image.prefab</param>
    /// <param name="replacePath">Art/Scences/LoadScence</param>
    /// <returns></returns>
    public static string GetBundlePath(FileInfo file, string replacePath) {
        int count = replacePath.LastIndexOf("/");
        string scenceHead = replacePath.Substring(count + 1, replacePath.Length - count - 1);
        Debug.Log(" scenceHead " + scenceHead);

        string tmpPath = file.FullName;
       
        tmpPath = FixedPath(tmpPath);
        Debug.Log(" tmpPath " + tmpPath + " replacePath" + replacePath);

        int tmpCount = tmpPath.IndexOf(replacePath);
        tmpCount += replacePath.Length + 1;
        Debug.Log(" FileName " + file.Name);
        int nameCount = tmpPath.LastIndexOf(file.Name);
     
        int tmpLength = nameCount - tmpCount;
        if (tmpLength > 0)
        {
            string subString = tmpPath.Substring(tmpCount, tmpPath.Length - tmpCount);
            Debug.Log("SubString " + subString);
            string[] result = subString.Split("/".ToCharArray());

            Debug.Log("result  " + scenceHead + "/" + result[0]);
            return scenceHead + "/" + result[0];
        }
        else {
            return scenceHead;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tmpFile">文件完整路径  D: UnityFrame / Assets / Art / Scences / LoadScence / LoadUI / Image.prefab</param>
    /// <param name="replacePath">相对路径 Art/Scences/LoadScence</param>
    /// <param name="writer"></param>
    public static void ChangeMark(FileInfo tmpFile, string replacePath, Dictionary<string, string> writer) {
        if (tmpFile.Extension == ".meta") {
            return;
        }
        string markStr = GetBundlePath(tmpFile, replacePath);
        MarkAsset(tmpFile, markStr, writer);
    }
    /// <summary>
    /// 对相应的file（bundle）进行标记
    /// </summary>
    /// <param name="file">file信息</param>
    /// <param name="markStr">标记内容</param>
    public static void MarkAsset(FileInfo file, string markStr,Dictionary<string,string> writer) {
        string fullPath = file.FullName;
        int assetCount = fullPath.IndexOf("Assets");
        string assetPath = fullPath.Substring(assetCount, fullPath.Length - assetCount);
        AssetImporter importer = AssetImporter.GetAtPath(assetPath);
        importer.assetBundleName = markStr;
        if (file.Extension == ".unity")
        {
            importer.assetBundleVariant = "u3d";
        }
        else {
            importer.assetBundleVariant = "ld";
        }
        string bundleName = "";
        string[] subMark = markStr.Split("/".ToCharArray());
        if (subMark.Length > 1)
        {
            bundleName = subMark[1];
        }
        else {
            bundleName = markStr;
        }
      string bundlePath  = bundleName.ToLower() + "." + importer.assetBundleVariant;
        if (!writer.ContainsKey(bundleName)) {
            writer.Add(bundleName, bundlePath);
        }
    }
}
