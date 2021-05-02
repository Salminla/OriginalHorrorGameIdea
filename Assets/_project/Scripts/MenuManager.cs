using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Toggle flyToggle;

    private bool flyState;
    
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        flyToggle.onValueChanged.AddListener(delegate(bool arg0) { SetFly(); });
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    void SetFly()
    {
        PlayerPrefs.SetInt("fly", Convert.ToInt16(flyToggle.isOn));
    }
}
