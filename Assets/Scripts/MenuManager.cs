using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    [SerializeField] TMP_Dropdown _resolutions;
    

    private void Awake()
    {
        LoadScreenPrefrences();
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
        if (ShouldLoadData)
        {
            GameObject loadingManager = GameObject.Find("Save Manager");
            LoadingManager lManager = loadingManager.GetComponent<LoadingManager>();
            loadingFinished = lManager.LoadSaveData();
        }
        LoadPrefData();     
        

        LoadingScenePanel.SetActive(false);
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// loaded on Awake. 
    /// </summary>
    void LoadScreenPrefrences()
    {
        ChangeDisplayType();
        ChangeScreenResolution();
    }
    /// <summary>
    /// Loaded when the new scene loads. 
    /// </summary>
    /// <returns></returns>
    private void LoadPrefData()
    {
        //Handle Shadows 
        light = GameObject.Find("Directional Light");

        int shadowRes = PlayerPrefs.GetInt("Shadow Resolution"); 
        if(shadowRes != 0)
        {
            switch (shadowRes)
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
        }

        //Handle Post Processing. 

        //Handle Mouse Speed
        float mouse = PlayerPrefs.GetFloat("Mouse Speed");
        if(mouse != 0)
        {
            GameObject.Find("Player").GetComponent<S_Player>().mouseSensitivity = mouse;
        }
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
                    PlayerPrefs.SetInt("Shadow Resolution", 0); 
                    break;
                }               
            case 1:
                {
                    light.GetComponent<Light>().shadowResolution = UnityEngine.Rendering.LightShadowResolution.Medium; 
                    PlayerPrefs.SetInt("Shadow Resolution", 1); 
                    break;
                }               
            case 2:
                {
                    light.GetComponent<Light>().shadowResolution = UnityEngine.Rendering.LightShadowResolution.High;
                    PlayerPrefs.SetInt("Shadow Resolution", 2);
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

    public void ChangeMouseSpeed(Slider speed)
    {
        //GameObject.Find("Player").GetComponent<S_Player>().mouseSensitivity = speed.value;
        PlayerPrefs.SetFloat("Mouse Speed", speed.value);
    }

    public void UpdatePossibleResolutions()
    {
        _resolutions.ClearOptions();
        Resolution[] resolutions = Screen.resolutions;
        List<String> availableResolutions = new List<String>();
        // Print the resolutions
        foreach (var res in resolutions)
        {
            //Debug.Log(res.width + "x" + res.height + " : " + res.refreshRateRatio);
            availableResolutions.Add(res.width + "x" + res.height);
        }
        _resolutions.AddOptions(availableResolutions);
    }
    /// <summary>
    /// Changes the screen resolution (ie. 1920x1080) via settings page on the menu. At the start of the game we pull the settings from PlayerPrefs.
    /// </summary>
    /// <param name="screenResolution"></param>
    public void ChangeScreenResolution(TMP_Dropdown screenResolution = null)
    {        
        
        if(screenResolution != null)
        {
            Debug.Log(screenResolution.options.ElementAt(screenResolution.value).text);
            string[] res = screenResolution.options.ElementAt(screenResolution.value).text.Split("x");
            Screen.SetResolution(int.Parse(res[0]), int.Parse(res[1]), Screen.fullScreenMode);
            PlayerPrefs.SetInt("Screen Width", int.Parse(res[0]));
            PlayerPrefs.SetInt("Screen Height", int.Parse(res[1]));
            PlayerPrefs.Save();
        }
        else
        {
            int width = PlayerPrefs.GetInt("Screen Width");
            int height = PlayerPrefs.GetInt("Screen Height");
            if(width != 0 && height != 0)
            {
                Screen.SetResolution(width, height, Screen.fullScreenMode);
            }
            else
            {
                Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
            }
            
        }
       

    }
    public void ChangeDisplayType(TMP_Dropdown value = null)
    {       
        if(value != null)
        {
            switch (value.value)
            {
                case 0:
                    {
                        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                        PlayerPrefs.SetInt("Display Type", 0);
                        break;
                    }
                case 1:
                    {
                        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                        PlayerPrefs.SetInt("Display Type", 1);
                        break;
                    }
                case 2:
                    {
                        Screen.fullScreenMode = FullScreenMode.Windowed;
                        PlayerPrefs.SetInt("Display Type", 2);
                        break;
                    }
                default:
                    {
                        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                        break;
                    }
            }
            PlayerPrefs.Save();
        }
        else
        {
            int num = PlayerPrefs.GetInt("Display Type");
            if (num != 0)
            {
                switch (num)
                {
                    case 0:
                        {
                            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                            break;
                        }
                    case 1:
                        {
                            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                            break;
                        }
                    case 2:
                        {
                            Screen.fullScreenMode = FullScreenMode.Windowed;
                            break;
                        }
                    default:
                        {
                            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                            break;
                        }
                }
            }
            else
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;


        }       
    }
    public void ChangeAudioLevel(Slider NewVolume)
    {
        AudioListener.volume = NewVolume.value;
        PlayerPrefs.SetFloat("Volume", NewVolume.value);
    }
}
