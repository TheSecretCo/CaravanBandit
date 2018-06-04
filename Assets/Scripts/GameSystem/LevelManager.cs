using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PrefabAttribute("Singleton/LevelManager")]
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    string levelDataPath;

    LevelConfigData levelConfigData;

    public void Init ()
    {
    }

    private void Awake ()
    {
        LoadLevelData();
    }

    // Use this for initialization
    void Start ()
    {
        //LoadLevelData();
    }

    void LoadLevelData ()
    {
        TextAsset configTextAsset = Resources.Load<TextAsset>(levelDataPath);
        levelConfigData = GameUtility.DeserializeFromByte<LevelConfigData>(configTextAsset.bytes);

        //string path = GameUtility.GetStreamingassetPath(levelDataPath);
        //StartCoroutine(LoadLevelDataRoutine(path,(LevelConfigData _levelConfigData) => { 
        //    levelConfigData = _levelConfigData; 
        //}));

        //levelConfigData = GameUtility.DeserializeFromPath<LevelConfigData>(path);
        //Debug.LogError(levelConfigData);
    }

    //IEnumerator LoadLevelDataRoutine (string _path, Action<LevelConfigData> _callback)
    //{
    //    WWW request = new WWW("file://" + _path);
    //    while (!request.isDone)
    //    {
    //        yield return null;
    //    }

    //    LevelConfigData result = GameUtility.DeserializeFromByte<LevelConfigData>(request.bytes);

    //    if (_callback != null)
    //    {
    //        _callback(result);
    //    }
    //}

    //void OnLevelDataLoadDone (LevelConfigData _levelConfigData)
    //{
    //    levelConfigData = _levelConfigData;
    //}

    public LevelConfig GetLevel (int _level)
    {
        return levelConfigData.levels.Find(item => item.level.Equals(_level));
    }
}
