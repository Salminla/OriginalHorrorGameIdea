using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text sprintValue;
    [SerializeField] private TMP_Text noteValue;
    [SerializeField] private Button quitButton;
    
    private void Start()
    {
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (sprintValue != null)
        {
            float val = GameManager.instance.sprintStrength;
            sprintValue.text = Mathf.RoundToInt(val).ToString();
            sprintValue.color = val > 50 ? Color.white : Color.red;
        }

        if (noteValue != null)
        {
            noteValue.text = "Notes: " + GameManager.instance.noteCount;
        }
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
