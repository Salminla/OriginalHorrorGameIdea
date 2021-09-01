using DG.Tweening;
using UnityEngine;

public class Drawer : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 finalOffset;
    [SerializeField] private float moveTime = 2f;
    
    private Vector3 originalPos;
    private Vector3 offsetPos;
    private bool doorState;

    private void Start()
    {
        originalPos = transform.position;
        offsetPos = transform.position + finalOffset;
    }

    private void Open()
    {
        transform.DOMove(!doorState ? offsetPos : originalPos, moveTime);
        doorState = !doorState;
    }
    
    public void Interact()
    {
        Open();
    }
}
