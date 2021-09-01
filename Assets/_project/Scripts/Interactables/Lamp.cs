using UnityEngine;

public class Lamp : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] lightSources;

    private void Switch()
    {
        foreach (var currLight in lightSources)
        {
            var enabled1 = currLight.activeSelf;
            enabled1 = !enabled1;
            currLight.SetActive(enabled1);
        }
    }

    public void Interact()
    {
        Switch();
    }
}
