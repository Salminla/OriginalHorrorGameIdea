using System.Collections;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { private set; get; }

    [SerializeField] private Player player;
    [SerializeField] private Monster monster;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private AudioClip bgMusic;
    
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Init()
    {
        Application.targetFrameRate = 30;
        AudioManager.instance.PlayMusic(bgMusic, true);
    }
    public void GameEnd()
    {
        player.enableMovement = false;
        endScreen.SetActive(true);
        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #endif
        
    }
}