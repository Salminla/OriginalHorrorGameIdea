using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button vrStartButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Toggle flyToggle;
    
    [Header("Panels")] 
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainPanel;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        vrStartButton.onClick.AddListener(StartGameVR);
        settingsButton.onClick.AddListener(OpenSettings);
        flyToggle.onValueChanged.AddListener(delegate { SetFly(); });
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("vr", Convert.ToInt16(false));
    }
    void StartGameVR()
    {
        PlayerPrefs.SetInt("vr", Convert.ToInt16(true));
        SceneManager.LoadScene(1);
    }

    void SetFly()
    {
        PlayerPrefs.SetInt("fly", Convert.ToInt16(flyToggle.isOn));
    }

    void OpenSettings()
    {
        settingsPanel.GetComponent<TweenInOut>().OpenUIPanel();
    }
}
