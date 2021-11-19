using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractableUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Events that other classes can subscribe to
    public delegate void MouseEvent(GameObject interactedUI, PointerEventData eventData);
    public MouseEvent OnEnter;
    public MouseEvent OnExit;
    public MouseEvent OnClicked;

    [Header("References")]
    public GameObject mainGameObject;
    public GameObject highlightUI;
    public Image icon;

    private Color initialIconColor;
    private bool inside = false;

    private bool interactable = true;
    public bool Interactable { get => interactable; 
        set {
            if (value)
            {
                
            } else
            {
                if (highlightUI != null)
                    highlightUI.SetActive(false);

                if (icon != null)
                {
                    icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0.5f);
                }
            }
        } 
    }

    private void Start()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Interactable && inside)
        {
            if (OnClicked != null)
                OnClicked(mainGameObject, eventData);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inside = true;
        if (Interactable)
        {
            if (highlightUI != null)
                highlightUI.SetActive(true);

            if (OnEnter != null)
                OnEnter(mainGameObject, eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inside = false;
        if (Interactable)
        {
            if (highlightUI != null)
                highlightUI.SetActive(false);

            if (OnExit != null)
                OnExit(mainGameObject, eventData);
        }
    }
}