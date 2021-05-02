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
        if (player != null)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 2f)
            {
                GameManager.instance.noteCount++;
                AudioManager.instance.Play(paperSound);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(3f);
        player = FindObjectOfType<Player>();
    }
}
