using DG.Tweening;
using UnityEngine;

public class TweenInOut : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    
    private RectTransform rectTransform;
    private Vector2 origPos;
    private Vector2 destPos;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        origPos = rectTransform.anchoredPosition;
        destPos = rectTransform.anchoredPosition * -1;
    }

    public void CloseUIPanel()
    {
        rectTransform.DOKill();
        rectTransform.DOAnchorPosX(destPos.x, duration)
            .OnComplete(() => gameObject.SetActive(false));
    }
    public void OpenUIPanel()
    {
        gameObject.SetActive(true);
        rectTransform.anchoredPosition = destPos;
        rectTransform.DOKill();
        rectTransform.DOAnchorPosX(origPos.x, duration);
    }
}
