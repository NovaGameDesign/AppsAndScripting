using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("UI Related")]
    [SerializeField] Scene MainScene;
    [SerializeField] LoadingManager LoadingManager;
    [SerializeField] GameObject LoadingScenePanel;
    bool loadingFinished = false;
    bool DoneLoadingPrefs = false;
    GameObject light;
    [SerializeField] TMP_Dropdown dropDown;
    

    private void Awake()
    {
        DontDestroyOnLoad(this);
        dropDown.value = int.Parse(PlayerPrefs.GetString("Shadow Resolution"));
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
        LoadingScenePanel.SetActive(true);
        while (!asyncLoad.isDone)
        {            
            yield return null;
        }
        LoadPrefData();
        if (ShouldLoadData)
        {
            GameObject loadingManager = GameObject.Find("Save Manager");
            LoadingManager lManager = loadingManager.GetComponent<LoadingManager>();
            loadingFinished = lManager.LoadSaveData();
        }
        

        LoadingScenePanel.SetActive(false);
        this.gameObject.SetActive(false);
    }

    private bool LoadPrefData()
    {
        //Handle Shadows 
        light = GameObject.Find("Directional Light");

        int shadowRes = int.Parse(PlayerPrefs.GetString("Shadow Resolution")); 
        switch(shadowRes)
        {
            case 0:
                {
                    light.GetComponent<Light>().shadowResolution = UnityEngine.Rendering.LightShadowResolution.Low;                 
                    break;
                }
            case 1:
                {
                    light.GetComponent<Light>().shadowResolution = UnityEngine.Rendering.LightShadowResolution.Medium;                  
                    break;
                }
            case 2:
                {
                    light.GetComponent<Light>().shadowResolution = UnityEngine.Rendering.LightShadowResolution.High;               
                    break;
                }
            case 3:
                {
                    light.GetComponent<Light>().shadowResolution = UnityEngine.Rendering.LightShadowResolution.VeryHigh;                  
                    break;
                }
            default:
                {
                    light.GetComponent<Light>().shadowResolution = UnityEngine.Rendering.LightShadowResolution.Medium;
                    break;
                }
        }


        //Handle Post Processing. 

        return false; 
    }
    public void LoadMenuScene()
    {
        StartCoroutine(LoadAsyncMenuScene());
        Time.timeScale = 1;
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

    /// <summary>
    /// Set the directional light shadow level. 
    /// </summary>
    /// <param name="resolution"></param>
    public void SetShadowLevel(TMP_Dropdown value)
    {
        light = GameObject.Find("Directional Light");
        int resolution = value.value;
        Debug.Log("The shadow value is: " + resolution);
        switch (resolution)
        {
            case 0:
                {
                    light.GetComponent<Light>().shadowResolution = UnityEngine.Rendering.LightShadowResolution.Low; 
                    PlayerPrefs.SetString("Shadow Resolution", "0"); 
                    break;
                }               
            case 1:
                {
                    light.GetComponent<Light>().shadowResolution = UnityEngine.Rendering.LightShadowResolution.Medium; 
                    PlayerPrefs.SetString("Shadow Resolution", "1"); 
                    break;
                }               
            case 2:
                {
                    light.GetComponent<Light>().shadowResolution = UnityEngine.Rendering.LightShadowResolution.High;
                    PlayerPrefs.SetString("Shadow Resolution", "2");
                    break;
                }                
            case 3:
                {
                    light.GetComponent<Light>().shadowResolution = UnityEngine.Rendering.LightShadowResolution.VeryHigh; 
                    PlayerPrefs.SetString("Shadow Resolution", "3"); 
                    break;
                }                
        }
        PlayerPrefs.Save();
    }
}
