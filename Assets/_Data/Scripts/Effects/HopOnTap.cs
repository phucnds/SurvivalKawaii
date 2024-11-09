using UnityEngine;
using UnityEngine.EventSystems;

public class HopOnTap : MonoBehaviour, IPointerDownHandler
{
    private RectTransform rt;
    private Vector2 initialPosition;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        initialPosition = rt.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        float targetY = initialPosition.y + Screen.height / 50;

        LeanTween.cancel(gameObject);
        LeanTween.moveY(rt, targetY, .6f).setEase(LeanTweenType.punch).setIgnoreTimeScale(true);
    }
}
