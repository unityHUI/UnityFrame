using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PathTool
{
    public static string GetFoldNameWithPlatForm()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                return "Mac";
            default:
                return "Other";
        }
    }
    private static string GetAppFilePath()
    {
        string path = "";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            path = Application.streamingAssetsPath;
        }
        else
        {
            path = Application.persistentDataPath;
        }
        return path;
    }
    public static string GetBundlePath()
    {
        string foldName = GetFoldNameWithPlatForm();
        string allPath = GetAppFilePath() + "/" + foldName;
        return allPath;
    }

    public static string GetWWWAssetBundlePath() {
        string tmpStr = "";
        if (Application.platform == RuntimePlatform.WindowsEditor && Application.platform == RuntimePlatform.OSXEditor)
        {
            tmpStr = "file:///" + GetBundlePath();
        }
        else {
            string tmpPath = GetBundlePath();
#if UNITY_ANDROID
            tmpStr = "jar:file://"+tmpPath
#elif UNITY_STANDALONE_WIN
            tmpStr = "file:///" + tmpPath;
#else
            tmpStr = "file://"+tmpPath;
#endif
        }
        return tmpStr;
    }
}
