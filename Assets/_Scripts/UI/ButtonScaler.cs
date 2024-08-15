using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text buttonText;
    private float hoverScale = 1.2f;
    private Color hoverColor = Color.white;
    private Vector3 originalScale;
    private Color originalColor;


    void Start()
    {
        originalScale = buttonText.transform.localScale;
        originalColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.transform.localScale = originalScale * hoverScale;
        buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.transform.localScale = originalScale;
        buttonText.color = originalColor;
    }
}
