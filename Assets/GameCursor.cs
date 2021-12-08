using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameCursor : MonoBehaviour, IPointerEnterHandler
{
    [Header("Cursor")]
    public Texture2D cursorTexture;
    public Texture2D cursorCrossTexture;
    public Vector2 offset;
    private CursorMode cursorMode = CursorMode.ForceSoftware;
    private Vector2 hotSpot = Vector2.zero;
    private Texture2D currentCursor;
    private bool cursorActive;

    private void Start()
    {
        currentCursor = cursorTexture;
    }

    //Method to set the cursor to a cross and back to normal
    public void setCrossCursor(bool b)
    {
        if (b)
        {
            currentCursor = cursorCrossTexture;
        }
        else if (!b)
        {
            currentCursor = cursorTexture;
        }
        //update cursor only if cursor is active!
        if (cursorActive)
        {
            Cursor.SetCursor(currentCursor, hotSpot + offset, cursorMode);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(currentCursor, hotSpot + offset, cursorMode);
        cursorActive = true;
    }

    public void OnPointerExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        cursorActive = false;
    }
}
