using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public BuildTipDisplay display;

    public GameObject represent;

    void OnDisable()
    {
        display.Hide();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        display.Display(represent.GetComponent<Building>());
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        display.Hide();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        display.Hide();
    }
}
