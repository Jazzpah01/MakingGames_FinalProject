using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InteractableUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Flags]
    public enum MouseClickEvent
    {
        None = 0,
        LeftMouseButton = 1,
        RightMouseButton = 2,
        MiddleMouseButton = 4
    }

    // Events that other classes can subscribe to
    public delegate void MouseEvent(GameObject interactedUI, PointerEventData eventData);
    public MouseEvent OnEnter;
    public MouseEvent OnExit;
    public MouseEvent OnClicked;

    [Header("Options")]
    [Tooltip("If enabled, then the highligh image will replace the default image.")]
    public bool replaceWithHighlight;
    [Tooltip("If enabled, then the toggle image will replace the default image and highlight image.")]
    public bool replaceWithToggledImage;
    [Tooltip("If this is a button, specify the mouse buttons to use.")]
    public MouseClickEvent mouseButton;

    [Header("References")]
    [Tooltip("The main/parent GameObject of a UI element.")]
    public GameObject mainGameObject;
    public Image image;
    public Image highlightImage;
    public Image toggledImage;

    private Color initialImageColor;
    private bool inside = false;

    // Use this for player controllers.
    public static bool UIInteraction = false;

    private bool interactable = true;
    private bool toggled = false;

    public bool Interactable { get => interactable; 
        set {
            if (value)
            {
                if (image != null)
                {
                    image.color = initialImageColor;
                }
            } else
            {
                SetHighlighted(inside);

                if (image != null)
                {
                    image.color = new Color(image.color.r / 2, image.color.g / 2, image.color.b / 2, image.color.a / 2);
                }
            }
        } 
    }

    public bool Toggled
    {
        get => toggled;

        set
        {
            if (toggledImage == null)
                throw new Exception("There is no toggled image to toggle.");

            // TODO: finish this
        }
    }

    private void Start()
    {
        if (image != null)
            initialImageColor = image.color;

        if (mainGameObject == null)
            throw new Exception("Cannot have an interactable UI with no main GameObject.");
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
            SetHighlighted(inside);

            if (OnEnter != null)
                OnEnter(mainGameObject, eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inside = false;
        if (Interactable)
        {
            SetHighlighted(inside);

            if (OnExit != null)
                OnExit(mainGameObject, eventData);
        }
    }

    private void SetHighlighted(bool highlight)
    {
        // Sanitise input
        if (highlightImage == null)
            return;

        highlightImage.gameObject.SetActive(highlight);

        if (replaceWithHighlight)
            image.gameObject.SetActive(!highlight);
    }
}