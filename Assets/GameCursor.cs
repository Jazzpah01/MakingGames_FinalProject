using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameCursor : MonoBehaviour, IPointerEnterHandler
{
    [Header("Cursor")]
    public Texture2D cursorTexture;
    public Vector2 offset;
    private CursorMode cursorMode = CursorMode.ForceSoftware;
    private Vector2 hotSpot = Vector2.zero;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot + offset, cursorMode);
    }

    public void OnPointerExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
