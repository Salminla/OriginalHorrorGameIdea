using UnityEngine;

public class CompositeInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private bool playSound;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private GameObject[] interactables;
    public void Interact()
    {
        if (playSound)
        {
            AudioManager.instance.Play(_clip);
        }
        foreach (var interactable in interactables)
        {
            var interaction = interactable.GetComponent<IInteractable>();
            interaction.Interact();
        }
    }
}
