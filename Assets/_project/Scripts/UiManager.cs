using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    
    
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
