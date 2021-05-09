using System.Collections;
using UnityEngine;

public class Note : MonoBehaviour
{
    public Player player;
    [SerializeField] private AudioClip paperSound;
    
    void Start()
    {
        StartCoroutine(GetPlayer());
    }
    
    void Update()
    {
        if (player == null) return;
        if (!(Vector3.Distance(player.transform.position, transform.position) < 2f)) return;
        GameManager.instance.noteCount++;
        if (AudioManager.instance != null)
            AudioManager.instance.Play(paperSound);
        else
            Debug.Log("Audio manager is not present!");

        Destroy(gameObject);
    }

    IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(3f);
        player = FindObjectOfType<Player>();
    }
}
