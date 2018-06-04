using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtility : MonoBehaviour
{
    public static int CountDigits (int number)
    {
        // In case of negative numbers
        number = Math.Abs(number);
        if (number >= 10)
        {
            return CountDigits(number / 10) + 1;
        }
        return 1;
    }

    public static IEnumerator LoadSceneRoutine (string _sceneName, LoadSceneMode _mode = LoadSceneMode.Single, Action _callback = null)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Single);
        while (!asyncOperation.isDone)
        {
            //Debug.LogError(asyncOperation.progress);
            yield return null;
        }

        //if (_callback != null)
        //{
        //    _callback();
        //}

        //if (_mode.Equals(LoadSceneMode.Single))
        //{
        //    SceneManager.UnloadSceneAsync(activeScene);
        //}
    }

    public static string GetStreamingassetPath (string _path)
    {
        System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
        stringBuilder.Append(Application.dataPath + "/StreamingAssets/");

#if !UNITY_EDITOR
#if UNITY_IOS
        stringBuilder.Append(Application.dataPath + "/Raw/");
#endif
#if UNITY_ANDROID
        stringBuilder.Append("jar:file://" + Application.dataPath + "!/assets/");
#endif
#endif
        stringBuilder.Append(_path);
        return stringBuilder.ToString();
    }

    public static T DeserializeFromPath<T> (string _path)
    {
        
        using (var fileStream = File.Open(_path, FileMode.Open, FileAccess.Read))
        {
            T result = GameDevWare.Serialization.MsgPack.Deserialize<T>(fileStream);
            return result;
        }
    }

    public static T DeserializeFromByte<T>(byte[] _bytes)
    {
        MemoryStream stream = new MemoryStream(_bytes);
        T result = GameDevWare.Serialization.MsgPack.Deserialize<T>(stream);
        return result;
    }
}
