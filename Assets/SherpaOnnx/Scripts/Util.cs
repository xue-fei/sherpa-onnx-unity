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
            return Util.GetPath();
        }
        else if (Application.platform == RuntimePlatform.Android
                  || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return Application.persistentDataPath;
        }
        else
        {
            return Util.GetPath();
        }
    }
}