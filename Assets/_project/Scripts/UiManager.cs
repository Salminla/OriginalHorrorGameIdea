using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> deathScreen;
    [SerializeField] private List<GameObject> finishedScreen;
    [SerializeField] private List<TMP_Text> sprintValue;
    [SerializeField] private List<TMP_Text> noteValue;
    [SerializeField] private Button quitButton;
    [SerializeField] private EventSystem uiSystem;

    private PlayerVRUi _vrUi;

    private void Start()
    {
        
        if (GameManager.instance.VRActive && uiSystem != null)
            uiSystem.gameObject.SetActive(false);
        
        if (GameManager.instance.VRActive)
        {
            _vrUi = FindObjectOfType<PlayerVRUi>();
            deathScreen.Add(_vrUi.endPanel);
            finishedScreen.Add(_vrUi.finishPanel);
            sprintValue.Add(_vrUi.sprint);
            noteValue.Add(_vrUi.notes);
        }
        quitButton.onClick.AddListener(Application.Quit);
    }

    void Update()
    {
        if (sprintValue != null)
        {
            float val = GameManager.instance.sprintStrength;
            foreach (var text in sprintValue)
            {
                if (text != null)
                {
                    text.text = Mathf.RoundToInt(val).ToString();
                    text.color = val > 50 ? Color.white : Color.red;
                }
            }
        }

        if (noteValue != null)
        {
            foreach (var text in noteValue)
            {
                if (text != null)
                    text.text = "Notes: " + GameManager.instance.noteCount;
            }
        }
    }

    public void ActivateEndScreen()
    {
        foreach (var o in deathScreen)
        {
            if (o != null)
                o.SetActive(true);
        }
    }

    public void ActivateFinishedScreen()
    {
        foreach (var o in finishedScreen)
        {
            if (o != null)
                o.SetActive(true);
        }
    }
}
