using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    [SerializeField] private int scrollSpeed = 2;

    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        rt.anchoredPosition = rt.anchoredPosition.With(y: 0);
    }

    private void Update()
    {
        rt.anchoredPosition += Vector2.up * Time.deltaTime * scrollSpeed;
    }
}
