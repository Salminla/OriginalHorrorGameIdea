using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Button returnButton;
    [SerializeField] private AudioMixer audioMixer;
    [Header("UI Elements")]
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Slider sensSlider;
    [SerializeField] private Slider mainVolume;
    [SerializeField] private TMP_Text fovText;
    [SerializeField] private TMP_Dropdown resolution;
    [SerializeField] private TMP_Dropdown windowMode;
    
    [SerializeField] private Toggle fullscreen;

    private Resolution[] _resolutions;
    private bool initialResSet;

    void Start()
    {
        returnButton.onClick.AddListener(ClosePanel);
        if (fovSlider != null) fovSlider.onValueChanged.AddListener(delegate { FovChanged(); });
        if (sensSlider != null) sensSlider.onValueChanged.AddListener(delegate { SensChanged(); });
        if (resolution != null) resolution.onValueChanged.AddListener(delegate { SetResolution(resolution.value); });
        if (windowMode != null) windowMode.onValueChanged.AddListener(delegate { SetWindowMode(); });
        if (fullscreen != null) fullscreen.onValueChanged.AddListener(delegate { SetFullscreen(); });
        if (mainVolume != null) mainVolume.onValueChanged.AddListener(delegate { SetVolume(); });
        
        InitResolutions();
        fovText.text = fovSlider.value.ToString();
    }

    private void OnEnable()
    {
        if (fullscreen != null) fullscreen.isOn = Screen.fullScreen;
        if (windowMode != null) InitWindowMode();
        returnButton.interactable = true;
    }

    private void InitResolutions()
    {
        _resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int resIndex = 0;
        
        resolution.ClearOptions();
        for (var i = 0; i < _resolutions.Length; i++)
        {
            var res = _resolutions[i];
            options.Add(res.ToString());
            
            if (Screen.width == res.width && Screen.height == res.height) 
                resIndex = i;
        }
        resolution.AddOptions(options);
        resolution.value = resIndex; 
        resolution.RefreshShownValue();
    }

    void SetResolution(int index)
    {
        Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, Screen.fullScreen);
    }

    void InitWindowMode()
    {
        windowMode.value = Screen.fullScreenMode switch
        {
            FullScreenMode.ExclusiveFullScreen => 0,
            FullScreenMode.FullScreenWindow => 1,
            FullScreenMode.Windowed => 2,
            _ => windowMode.value
        };
    }
    void SetWindowMode()
    {
        Screen.fullScreenMode = windowMode.value switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.FullScreenWindow,
            2 => FullScreenMode.Windowed,
            _ => Screen.fullScreenMode
        };
    }

    void SetFullscreen()
    {
        Screen.fullScreen = fullscreen.isOn;
    }

    void SetVolume()
    {
        audioMixer.SetFloat("masterVol", mainVolume.value);
    }

    void FovChanged()
    {
        fovText.text = fovSlider.value.ToString();
        PlayerPrefs.SetInt("fov", Convert.ToInt32(fovSlider.value));
    }
    
    void SensChanged()
    {
        PlayerPrefs.SetInt("sensitivity", Convert.ToInt32(sensSlider.value));
    }

    void ClosePanel()
    {
        GetComponent<TweenInOut>().CloseUIPanel();
    }
}
