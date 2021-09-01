using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3;
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private float promptOffset = 1.8f;

    void Start()
    {
        if (interactPrompt.GetComponentInChildren(typeof(TMP_Text)) != null)
        {
            interactText = interactPrompt.GetComponentInChildren<TMP_Text>();
        }
    }

    void Update()
    {
        GameObject nearestGameObject = GetNearestGameObject();
        ShowInteractPrompt(nearestGameObject);
        if (nearestGameObject == null) return;
        if (Input.GetButtonDown("Fire1"))
        {
            var interactable = nearestGameObject.GetComponent<IInteractable>();
            if (interactable == null) return;
            interactable.Interact();
        }
    }
    
    private void ShowInteractPrompt(GameObject nearestGameObject)
    {
        if (nearestGameObject != null && nearestGameObject.GetComponent<IInteractable>() != null)
        {
            interactPrompt.gameObject.SetActive(true);
            var position = nearestGameObject.transform.position;
            float angleRelative = playerCamera.transform.rotation.eulerAngles.x;
            if (angleRelative > 180)
                angleRelative -= 360;
            angleRelative /= 70;
            interactPrompt.transform.position = new Vector3(position.x ,playerCamera.transform.position.y * -angleRelative + promptOffset ,position.z);
            interactText.text = "Use\n" + nearestGameObject.name;
        }
        else
        {
            interactPrompt.gameObject.SetActive(false);
        }
    }
    
    private GameObject GetNearestGameObject()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out var hit, interactDistance))
        {
            return hit.transform.gameObject;
        }
        return null;
    }
}
