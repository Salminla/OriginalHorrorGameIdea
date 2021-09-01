using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject objectToRotate;
    [SerializeField] private bool reverseDirection;
    [SerializeField] private float degreesToOpen = 90;
    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    
    private bool _doorIsOpen;
    private Quaternion _transformRotation;
    private Quaternion _originalRot;
    private Quaternion _newRot;

    private void Start()
    {
        if (objectToRotate == null)
        {
            objectToRotate = this.gameObject;
        }
        _originalRot = objectToRotate.transform.rotation;
        if (!reverseDirection)
            _newRot.eulerAngles = _originalRot.eulerAngles + Vector3.up * degreesToOpen;
        else
            _newRot.eulerAngles = _originalRot.eulerAngles + Vector3.up * -degreesToOpen;
    }

    public void Open()
    {
        var doorTransform = objectToRotate.transform;
        _transformRotation = doorTransform.rotation;
        if (!_doorIsOpen)
        {
            DOTween.Kill(objectToRotate.transform);
            objectToRotate.transform.DORotate(_newRot.eulerAngles, 2f);
            _doorIsOpen = true;
        }
        else
        {
            DOTween.Kill(objectToRotate.transform);
            objectToRotate.transform.DORotate(_originalRot.eulerAngles, 2f).OnComplete(() =>
            {
                AudioManager.instance.Play(closeSound);
            });
            _doorIsOpen = false;
        }
    }

    public void Interact()
    {
        Open();
    }
}
