using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Related")]
    [SerializeField] Scene MainScene;
    [SerializeField] LoadingManager LoadingManager;
    [SerializeField] GameObject LoadingScenePanel;


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void StartGame(bool LoadData)
    {
        // Use a coroutine to load the Scene in the background
        StartCoroutine(LoadGameScene(LoadData));

    }

    IEnumerator LoadGameScene(bool ShouldLoadData)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelOne");

        bool load = true;
        while (!asyncLoad.isDone)
        {
            LoadingScenePanel.SetActive(true);
            yield return null;
        }
        if (load)
        {

        }
        else
            LoadingScenePanel.SetActive(false);
    }

    public void LoadMenuScene()
    {
        StartCoroutine(LoadAsyncMenuScene());

    }
    IEnumerator LoadAsyncMenuScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
