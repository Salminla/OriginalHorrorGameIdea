using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { private set; get; }
    
    [SerializeField] private Player player;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private VolumeProfile globalVolume;
    [SerializeField] private bool vr = false;
    
    public float noteCount = 0;
    public float sprintStrength;
    public bool allowJump;
    public bool VRActive { get; private set; }

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
        VRActive = Convert.ToBoolean(PlayerPrefs.GetInt("vr"));
        //Application.targetFrameRate = 10;
#if UNITY_EDITOR
        VRActive = vr;
#endif

        if (_spawner != null)
            _spawner.HandleSpawning();
        
        if (FindObjectOfType<Player>() != null)
            player = FindObjectOfType<Player>();
        
        if (player != null)
            InitAudio();

        if (globalVolume != null && globalVolume.TryGet<FilmGrain>(out var filmGrain))
            filmGrain.active = !VRActive;

        allowJump = Convert.ToBoolean(PlayerPrefs.GetInt("fly"));
    }

    private void InitAudio()
    {
        if (player.GetComponent<AudioSource>() == null) return;
        AudioSource[] sources = player.GetComponents<AudioSource>();
        
        if (AudioManager.instance == null) return;
        AudioManager.instance.musicSource = sources[0];
        AudioManager.instance.effectsSource = sources[1];
        AudioManager.instance.PlayMusic(bgMusic, true);
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

    private void GameFinish()
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