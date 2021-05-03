using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button vrStartButton;
    [SerializeField] private Toggle flyToggle;

    private bool flyState;
    
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        vrStartButton.onClick.AddListener(StartGameVR);
        flyToggle.onValueChanged.AddListener(delegate(bool arg0) { SetFly(); });
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
}
