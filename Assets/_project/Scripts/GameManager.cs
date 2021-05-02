using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { private set; get; }
    
    [SerializeField] private Player player;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private AudioClip bgMusic;

    public float noteCount = 0;
    public float sprintStrength;
    public bool allowJump;

    private bool gameEnd;

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
        //Application.targetFrameRate = 30;
        if (_spawner != null)
            _spawner.HandleSpawning();
        
        if (FindObjectOfType<Player>() != null)
            player = FindObjectOfType<Player>();
        
        AudioSource[] sources = player.GetComponents<AudioSource>();
        AudioManager.instance.musicSource = sources[0];
        AudioManager.instance.effectsSource = sources[1];
        AudioManager.instance.PlayMusic(bgMusic, true);
        
        allowJump = Convert.ToBoolean(PlayerPrefs.GetInt("fly"));
    }

    private void Update()
    {
        if (noteCount >= 5 && !gameEnd)
        {
            GameFinish();
        }
    }

    public void GameDeath()
    {
        gameEnd = true;
        player.enableMovement = false;
        _uiManager.ActivateEndScreen();
        StartCoroutine(QuitGameDelayed());
    }

    public void GameFinish()
    {
        gameEnd = true;
        player.enableMovement = false;
        if (FindObjectOfType<Monster>() != null)
            FindObjectOfType<Monster>().gameObject.SetActive(false);    // Very bad
        _uiManager.ActivateFinishedScreen();
        Cursor.lockState = CursorLockMode.None;
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }
    IEnumerator QuitGameDelayed()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
        
    }
}