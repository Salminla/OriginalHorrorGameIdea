using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { private set; get; }
    
    [SerializeField] private Player player;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform monsterSpawnContainer;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject finishedScreen;
    [SerializeField] private AudioClip bgMusic;

    public float noteCount = 0;
    public float sprintStrength;
    public bool allowJump;

    private bool gameEnd;
    List<Transform> spawnPoints = new List<Transform>();
    
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
        HandleSpawning();
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

    // These to their own class ----
    private void GetSpawnPoints()
    {
        if (monsterSpawnContainer != null)
        {
            for (int i = 0; i < monsterSpawnContainer.childCount; i++)
            {
                spawnPoints.Add(monsterSpawnContainer.GetChild(i));
            }
        }
        else
            spawnPoints.Add(transform);

    }
    private void SpawnPlayer(Vector3 pos)
    {
        GameObject temp = Instantiate(playerPrefab, pos + Vector3.up, Quaternion.identity);
        player = temp.GetComponent<Player>();
    }
    private void SpawnMonster(Vector3 pos)
    {
        Instantiate(monsterPrefab, pos + Vector3.up, Quaternion.identity);
    }

    private void HandleSpawning()
    {
        GetSpawnPoints();
        Transform playerSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Transform monsterSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        while (playerSpawn == monsterSpawn)
            playerSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        if (playerPrefab != null)
            SpawnPlayer(playerSpawn.position);
        if (monsterPrefab != null)
            SpawnMonster(monsterSpawn.position);
    }
    // -------------------------------------
    
    public void GameDeath()
    {
        gameEnd = true;
        player.enableMovement = false;
        deathScreen.SetActive(true);
        StartCoroutine(QuitGameDelayed());
    }

    public void GameFinish()
    {
        gameEnd = true;
        player.enableMovement = false;
        if (FindObjectOfType<Monster>() != null)
            FindObjectOfType<Monster>().gameObject.SetActive(false);    // Very bad
        finishedScreen.SetActive(true);
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