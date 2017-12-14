using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PathTool
{
    private static string GetFoldNameWithPlatForm()
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
        string allPath = Path.Combine(GetAppFilePath(), foldName);
        return allPath;
    }
}
