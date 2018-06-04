using UnityEngine;

public class TitleCanvasScript : MonoBehaviour
{
    [SerializeField]
    string mainMenuSceneName;

    // Use this for initialization
    void Start ()
    {
        LevelManager.Instance.Init();
    }

    public void OnPlayButtonPressed ()
    {
        StartCoroutine(GameUtility.LoadSceneRoutine(mainMenuSceneName));
    }
}
