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

    [Flags]
    public enum InteractableUIOptions
    {
        None = 0,
        RemoveImageOnHighlight = 1,
        RemoveImageOnToggle = 2,
        RemoveHighlightOnToggle = 4
    }

    // Events that other classes can subscribe to
    public delegate void MouseEvent(GameObject interactedUI, PointerEventData eventData);
    public MouseEvent OnEnter;
    public MouseEvent OnExit;
    public MouseEvent OnClicked;

    [Header("Options")]
    //[Tooltip("If enabled, then the highligh image will replace the default image.")]
    //public bool replaceWithHighlight;
    //[Tooltip("If enabled, then the toggle image will replace the default image and highlight image.")]
    //public bool replaceWithToggledImage;
    public InteractableUIOptions options = InteractableUIOptions.None;
    [Tooltip("If this is a button, specify the mouse buttons to use.")]
    public MouseClickEvent mouseButton = MouseClickEvent.None;

    [Header("References")]
    [Tooltip("The main/parent GameObject of a UI element.")]
    public GameObject mainGameObject;
    public Image image;
    public Image highlightImage;
    public Image toggledImage;

    private Color initialImageColor;
    private bool inside = false;

    // Use this for player controllers.
    public static bool OnUI { 
        get {
            return (onUICount > 0);
        } 
    }
    private static int onUICount = 0;

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

            toggledImage.gameObject.SetActive(value);

            if (value)
            {
                if (options.HasFlag(InteractableUIOptions.RemoveHighlightOnToggle) && Highlighted)
                {
                    highlightImage.gameObject.SetActive(false);
                }
            } else
            {
                if (inside && !Highlighted)
                {
                    highlightImage.gameObject.SetActive(true);
                }
            }
        }
    }

    private bool Highlighted => (highlightImage != null && highlightImage.gameObject.activeSelf);

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
            if (OnClicked != null &&
                (eventData.pointerClick == PointerEventData.InputButton.Left && ))
                OnClicked(mainGameObject, eventData);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!inside)
            onUICount++;

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
        if (inside)
            onUICount--;

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

        if (options.HasFlag(InteractableUIOptions.RemoveImageOnHighlight))
            image.gameObject.SetActive(!highlight);
    }
}