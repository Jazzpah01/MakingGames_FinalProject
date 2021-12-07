using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollOnMouseover : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.instance;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameManager.hud.mouseOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        gameManager.hud.mouseOver = false;
    }

    private void OnDisable()
    {
        gameManager.hud.mouseOver = false;
    }
}
