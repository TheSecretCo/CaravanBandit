using System;
using System.Text;
using UnityEngine;

public class MainMenuCanvasScript : MonoBehaviour
{
    [SerializeField]
    string levelSceneNamePrefix;
    int currentLevel = 1;

    // Use this for initialization
    void Start ()
    {

    }

    public void OnPlayButtonPressed ()
    {
        string levelText = GetLevelText();
        StartCoroutine(GameUtility.LoadSceneRoutine(levelText));
    }

    string GetLevelText ()
    {
        int levelPrefixNumber = 5 - GameUtility.CountDigits(currentLevel);
        StringBuilder stringBuilder = new StringBuilder(levelSceneNamePrefix);
        for (int i = 0; i < levelPrefixNumber; i++)
        {
            stringBuilder.Append("0");
        }
        stringBuilder.Append(currentLevel);

        return stringBuilder.ToString();
    }
}
