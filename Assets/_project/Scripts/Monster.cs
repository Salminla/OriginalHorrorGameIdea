using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float moveSpeed = 10f;

    private bool atPlayerPos { set; get; }
    private Vector3 playerPos;

    private void Start()
    {
        if (player == null)
            StartCoroutine(GetPlayer());
    }

    void Update()
    {
        if (player != null)
            SeekPlayer();
    }

    void SeekPlayer()
    {
        playerPos = player.transform.position;

        if (!atPlayerPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos - Vector3.up / 0.9f, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, player.transform.rotation, 50 * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, playerPos) < 1.5f)
        {
            atPlayerPos = true;
            GameManager.instance.GameDeath();
        }
    }
    IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(3f);
        player = FindObjectOfType<Player>();
    }
}