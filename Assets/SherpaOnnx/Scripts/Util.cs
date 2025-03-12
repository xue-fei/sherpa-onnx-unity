using UnityEngine;

public static class Util
{
    public static string GetPath()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor
            || Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.LinuxEditor
             || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            return Application.streamingAssetsPath;
        }
        else if (Application.platform == RuntimePlatform.Android
                  || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return Application.persistentDataPath;
        }
        else
        {
            return Application.streamingAssetsPath;
        }
    }
}